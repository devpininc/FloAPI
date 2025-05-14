using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace TestFloApi.ConsoleApp
{
    public class TestRunner
    {
        private readonly HttpClient _client;

        public TestRunner()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7187") // your FloApi address
            };
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Starting FloApi test...");

            // Step 1: Send Magic Link
            var email = "sunil@homepin.ca";

            var sendLinkContent = new StringContent(JsonSerializer.Serialize(email), Encoding.UTF8, "application/json");
            var sendLinkResponse = await _client.PostAsync("/api/auth/send-link", sendLinkContent);

            Console.WriteLine($"[Send Link] Status: {sendLinkResponse.StatusCode}");

            Console.WriteLine("Check your API Console for Magic Link URL. Copy the token part.");
            Console.WriteLine("Paste the token here:");
            var token = Console.ReadLine();

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("No token entered. Exiting...");
                return;
            }

            // Step 2: Login with Magic Token
            var loginResponse = await _client.GetAsync($"/api/auth/login-with-token?token={Uri.EscapeDataString(token)}");
            var loginJson = await loginResponse.Content.ReadAsStringAsync();

            Console.WriteLine("[Login Response]");
            Console.WriteLine(loginJson);

            var loginResult = JsonSerializer.Deserialize<LoginResult>(loginJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (loginResult?.Token == null)
            {
                Console.WriteLine("[ERROR] Login failed.");
                return;
            }

            Console.WriteLine($"[Token] {loginResult.Token}");

            // Step 3: Create Agent
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);

            var newAgent = new
            {
                DisplayName = "Jane Realtor",
                Email = "jane@example.com",
                Phone = "416-123-4567",
                RecoNumber = "RE987654",
                TradeName = "Jane Realty Pro"
            };

            var createAgentContent = new StringContent(JsonSerializer.Serialize(newAgent), Encoding.UTF8, "application/json");
            var createAgentResponse = await _client.PostAsync("/api/users/agent", createAgentContent);

            Console.WriteLine($"[Create Agent] Status: {createAgentResponse.StatusCode}");
            var createAgentResult = await createAgentResponse.Content.ReadAsStringAsync();
            Console.WriteLine("[Create Agent Response]");
            Console.WriteLine(createAgentResult);
        }

        private class LoginResult
        {
            public string Token { get; set; }
            public UserInfo User { get; set; }
        }

        private class UserInfo
        {
            public string Id { get; set; }
            public string DisplayName { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
        }
    }
}
