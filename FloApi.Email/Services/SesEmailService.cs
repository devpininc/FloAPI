using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using FloApi.Email.Config;
using Microsoft.Extensions.Options;

namespace FloApi.Email.Services
{
    public class SesEmailService
    {
        private readonly AwsSesSettings _settings;
        private readonly IAmazonSimpleEmailService _sesClient;

        public SesEmailService(IOptions<AwsSesSettings> options, IAmazonSimpleEmailService sesClient)
        {
            _settings = options.Value;
            _sesClient = sesClient;
        }

        public async Task<bool> SendMagicLinkAsync(string toEmail, string loginUrl)
        {
            try
            {
                // Load HTML template and inject the login URL
                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "MagicLinkEmailTemplate.html");
                var htmlBody = File.ReadAllText(templatePath).Replace("{{LoginUrl}}", loginUrl);

                var sendRequest = new SendEmailRequest
                {
                    Source = $"{_settings.SES.FromName} <{_settings.SES.FromAddress}>",
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { toEmail }
                    },
                    Message = new Message
                    {
                        Subject = new Content("Your Magic Login Link to AgentFlo"),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = htmlBody
                            }
                        }
                    }
                };

                var response = await _sesClient.SendEmailAsync(sendRequest);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SES EMAIL ERROR] {ex.Message}");
                return false;
            }
        }



    }
}
