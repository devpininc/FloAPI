using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
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

        public void SendMagicLinkViaSmtp(string toEmail, string loginUrl)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("sunil@homepin.ca", "!!092311!!"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("sunil@homepin.ca"),
                Subject = "Your Magic Login Link to AgentFlo",
                Body = $"Click below to login:\n\n{loginUrl}",
                IsBodyHtml = false,
            };
            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);

            Console.WriteLine($"[EMAIL SENT] to {toEmail}");
        }

        public async Task<bool> SendMagicLinkAsync(string toEmail, string loginUrl, string accessToken)
        {
            try
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
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("[ERROR Sending Email]");
                    Console.WriteLine($"Status Code: {response.StatusCode}");
                    Console.WriteLine($"Error Message: {errorMessage}");
                }
                return response.IsSuccessStatusCode;
            
            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }
}
