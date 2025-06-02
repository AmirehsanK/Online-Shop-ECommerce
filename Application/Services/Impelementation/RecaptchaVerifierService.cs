using System.Text.Json;
using Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Impelementation;

public class RecaptchaVerifierService : IRecaptchaVerifier
{
    private readonly HttpClient _httpClient;
        private readonly string _recaptchaSecretKey;
        private const string GoogleRecaptchaVerifyApiUrl = "https://www.google.com/recaptcha/api/siteverify";

        /// <summary>
        /// Initializes a new instance of the <see cref="RecaptchaVerifierService"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory to create HTTP clients.</param>
        /// <param name="configuration">The application configuration to retrieve the reCAPTCHA secret key.</param>
        /// <exception cref="ArgumentNullException">Thrown if httpClientFactory or configuration is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the reCAPTCHA secret key is not configured.</exception>
        public RecaptchaVerifierService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            if (httpClientFactory == null)
            {
                throw new ArgumentNullException(nameof(httpClientFactory));
            }
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _httpClient = httpClientFactory.CreateClient("RecaptchaVerifier");
            _recaptchaSecretKey = configuration["GoogleRecaptcha:SecretKey"]
                                  ?? throw new InvalidOperationException("Google reCAPTCHA Secret Key is not configured. Please set 'GoogleRecaptcha:SecretKey' in configuration.");
        }

        /// <summary>
        /// Verifies the reCAPTCHA token asynchronously by sending a request to Google's API.
        /// </summary>
        /// <param name="recaptchaToken">The reCAPTCHA token (g-recaptcha-response) from the client.</param>
        /// <param name="userIpAddress">Optional: The user's IP address. Recommended for v2, not used for v3 by default by Google's API but can be sent.</param>
        /// <returns>True if the reCAPTCHA token is valid according to Google, otherwise false.</returns>
        public async Task<bool> IsRecaptchaValidAsync(string recaptchaToken, string? userIpAddress = null)
        {
            if (string.IsNullOrEmpty(recaptchaToken))
            {
                return false;
            }

            try
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("secret", _recaptchaSecretKey),
                    new KeyValuePair<string, string>("response", recaptchaToken),
                    // Include remoteip if you have it and want to send it.
                    // Google's documentation indicates it's optional.
                    // For v3, Google often infers this.
                    userIpAddress != null ? new KeyValuePair<string, string>("remoteip", userIpAddress) : new KeyValuePair<string, string>("",""),
                });

                HttpResponseMessage response = await _httpClient.PostAsync(GoogleRecaptchaVerifyApiUrl, content);
                response.EnsureSuccessStatusCode(); // Throw if not a success code.

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var verificationResponse = JsonSerializer.Deserialize<RecaptchaVerificationResponse>(jsonResponse);

                // For reCAPTCHA v3, you might also want to check the score and action.
                // For example: if (verificationResponse?.success == true && verificationResponse.score >= 0.5)
                // For simplicity, this example only checks 'success'.
                return verificationResponse?.success ?? false;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error verifying reCAPTCHA: {ex.Message}");
                return false;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing reCAPTCHA response: {ex.Message}");
                return false;
            }
            catch (Exception ex) // Catch-all for other unexpected errors
            {
                Console.WriteLine($"An unexpected error occurred during reCAPTCHA verification: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Represents the JSON response from Google's reCAPTCHA verification API.
        /// </summary>
        private class RecaptchaVerificationResponse
        {
            public bool success { get; set; }
            public DateTime challenge_ts { get; set; } // Timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
            public string? hostname { get; set; }      // The hostname of the site where the reCAPTCHA was solved
            public double score { get; set; }          // Only for reCAPTCHA v3: the score for this request (0.0 - 1.0)
            public string? action { get; set; }        // Only for reCAPTCHA v3: the action name for this request
            [System.Text.Json.Serialization.JsonPropertyName("error-codes")]
            public List<string>? errorcodes { get; set; } // Optional error codes
        }
    
}