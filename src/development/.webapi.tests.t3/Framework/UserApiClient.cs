using NBomber.CSharp;
using NBomber.Http.CSharp;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AccountAPI.Tests.T3.Models;

namespace AccountAPI.Tests.T3.Framework
{
    public static class ApiEndpoints
    {
        public const string Register = "/Account/register";
        public const string Login = "/Account/login";
    }

    public class UserApiClient
    {
        private readonly HttpClient _httpClient;

        public UserApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<NBomber.Contracts.Response<HttpResponseMessage>> RegisterUser(User user)
        {
            var userJson = JsonSerializer.Serialize(user);
            var content = new StringContent(userJson, Encoding.UTF8, "application/json");

            var request = Http.CreateRequest("POST", ApiEndpoints.Register)
                                  .WithBody(content);

            return await Http.Send(_httpClient, request);
        }

        public async Task<NBomber.Contracts.Response<HttpResponseMessage>> LoginUser(User user)
        {
            var userJson = JsonSerializer.Serialize(user);
            var content = new StringContent(userJson, Encoding.UTF8, "application/json");

            var request = Http.CreateRequest("POST", ApiEndpoints.Login)
                                  .WithBody(content);

            return await Http.Send(_httpClient, request);
        }
    }
}
