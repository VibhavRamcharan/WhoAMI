using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AccountAPI.Tests.T2.Framework
{
    public class WebHelper
    {
        private readonly HttpClient _client;

        public WebHelper(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string requestUri, T contentValue)
        {
            var json = JsonConvert.SerializeObject(contentValue);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _client.PostAsync(requestUri, content);
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await _client.GetAsync(requestUri);
        }

        public async Task<string> GetResponseContentAsync(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> CreateUserAsync(string username, string password)
        {
            var user = new Models.User { Username = username, Password = password };
            return await PostAsync("Account/register", user);
        }

        public async Task<HttpResponseMessage> DeleteUserAsync(string username)
        {
            return await _client.DeleteAsync($"Account/deleteuser?username={username}");
        }

        // You can add other HTTP methods (GET, PUT, DELETE) here as needed
    }
}
