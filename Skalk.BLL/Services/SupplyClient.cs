using Newtonsoft.Json;
using Nexar.Client.Token;
using Skalk.DAL.SupplyTypes;
using System.Net.Http.Headers;
using System.Text;

namespace Skalk.BLL.Services
{
    internal class SupplyClient : IDisposable
    {
        // access tokens expire after one day
        private static readonly TimeSpan tokenLifetime = TimeSpan.FromDays(1);

        // keep track of token and expiry time
        private static string? token = null;
        private static DateTime tokenExpiresAt = DateTime.MinValue;

        private readonly string clientId;
        private readonly string clientSecret;
        private readonly HttpClient httpClient;

        public SupplyClient()
        {
            clientId = "f3a998a6-8b83-4156-adb3-d2509ffebea4"
               ?? throw new InvalidOperationException("Please set environment variable 'NEXAR_CLIENT_ID'");
            clientSecret = "C1YYjOhYj7fzuHDVPTTeOpHOR74e8nbvWi6p"
                ?? throw new InvalidOperationException("Please set environment variable 'NEXAR_CLIENT_SECRET'");

            httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://api.nexar.com/graphql")
            };
        }

        public async Task<Response> RunQueryAsync(Request request)
        {
            // for another way of running GraphQL queries, see the related demo at:
            // https://github.com/NexarDeveloper/nexar-templates/tree/main/nexar-console-supply
            await EnsureValidTokenAsync();
            string requestString = JsonConvert.SerializeObject(request);
            HttpResponseMessage httResponse = await httpClient.PostAsync(httpClient.BaseAddress, new StringContent(requestString, Encoding.UTF8, "application/json"));
            httResponse.EnsureSuccessStatusCode();
            string responseString = await httResponse.Content.ReadAsStringAsync();
            Response response = JsonConvert.DeserializeObject<Response>(responseString);
            return response;
        }

        private async Task EnsureValidTokenAsync()
        {
            // get an access token, replacing the existing one if it has expired
            if (token == null || DateTime.UtcNow >= tokenExpiresAt)
            {
                tokenExpiresAt = DateTime.UtcNow + tokenLifetime;
                using HttpClient authClient = new();
                token = await authClient.GetNexarTokenAsync(clientId, clientSecret);
            }

            // set the default Authorization header so it includes the token
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
