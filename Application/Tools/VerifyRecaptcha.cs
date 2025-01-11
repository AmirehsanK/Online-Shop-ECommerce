using System.Text.Json.Nodes;

namespace Application.Tools;

public class VerifyRecaptcha
{
    public static async Task<bool> VerifyRecaptchaV3(string response, string secret, string verificationurl)
    {
        using (var client = new HttpClient())
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(response), "response");
            content.Add(new StringContent(secret), "secret");
            var result = await client.PostAsync(verificationurl, content);

            if (result.IsSuccessStatusCode)
            {
                var strResponse = await result.Content.ReadAsStringAsync();
                Console.WriteLine(strResponse);

                var jsonResponse = JsonNode.Parse(strResponse);
                if (jsonResponse != null)
                {
                    var success = (bool?)jsonResponse["success"];
                    if (success != null && success == true) return true;
                }
            }
        }

        return false;
    }
}