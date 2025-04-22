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

        public async Task<bool> SendMagicLinkAsync(string toEmail, string subject, string htmlContent, string accessToken)
        {
            var requestUrl = $"{_settings.ApiBaseUrl}emails/send";

            var payload = new
            {
                email_address = toEmail,
                subject = subject,
                html_content = htmlContent,
                from_email = "your_verified_sender@example.com", // Must be verified in Constant Contact
                from_name = "FloAPI"
            };

            var json = System.Text.Json.JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _http.PostAsync(requestUrl, content);
            return response.IsSuccessStatusCode;
        }
    }
}
