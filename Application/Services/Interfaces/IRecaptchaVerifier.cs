namespace Application.Services.Interfaces;

    /// <summary>
    /// Interface for verifying Google reCAPTCHA responses.
    /// </summary>
    public interface IRecaptchaVerifier
    {
        /// <summary>
        /// Verifies the reCAPTCHA token asynchronously.
        /// </summary>
        /// <param name="recaptchaToken">The reCAPTCHA token received from the client.</param>
        /// <param name="userIpAddress">Optional: The user's IP address.</param>
        /// <returns>True if the token is valid, otherwise false.</returns>
        Task<bool> IsRecaptchaValidAsync(string recaptchaToken, string? userIpAddress = null); 
    }