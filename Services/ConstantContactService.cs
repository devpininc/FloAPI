using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using FloAPI.Config;
using Microsoft.Extensions.Options;

namespace FloAPI.Services
{
    public class ConstantContactService
    {
        private readonly ConstantContactSettings _settings;
        private readonly HttpClient _http;

        public ConstantContactService(IOptions<ConstantContactSettings> options)
        {
            _settings = options.Value;
            _http = new HttpClient();
        }

        public async Task<bool> SendMagicLinkAsync(string toEmail, string loginUrl, string accessToken)
        {
            var requestUrl = $"{_settings.ApiBaseUrl}emails";

            // Load the email template and replace {{LoginUrl}}
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "MagicLinkEmailTemplate.html");
            var htmlContent = File.ReadAllText(templatePath).Replace("{{LoginUrl}}", loginUrl);

            var payload = new
            {
                personalizations = new[]
                {
                new
                {
                    to = new[]
                    {
                        new { email = toEmail }
                    },
                        subject = "Your Magic Login Link to AgentFlo"
                    }
                },
                from = new
                {
                    email = "jango.aws@gmail.com", // Replace with your real verified sender
                    name = "AgentFlo"
                },
                content = new[]
                {
                    new
                    {
                        type = "text/html",
                        value = htmlContent
                    }
                }
            };

            var json = System.Text.Json.JsonSerializer.Serialize(payload);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _http.PostAsync(requestUrl, httpContent);
            return response.IsSuccessStatusCode;
        }


    }
}
