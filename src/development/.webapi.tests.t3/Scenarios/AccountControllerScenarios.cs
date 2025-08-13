using System.Text;
using System.Text.Json;
using NBomber.CSharp;

namespace AccountAPI.Tests.T3.Scenarios
{
    internal class AccountControllerScenarios
    {
        record RegisterRequest(string Username, string Password);
        record LoginRequest(string Username, string Password);

        public NBomber.Contracts.ScenarioProps CreateRegisterScenario(HttpClient httpClient)
        {
            return Scenario.Create("RegisterEndpoint", async context =>
            {
                var baseUrl = "http://localhost:80/account/register";

                var username = $"user_{new Random().Next(1, int.MaxValue)}";
                var password = $"pass_{new Random().Next(1, int.MaxValue)}";

                var request = new RegisterRequest(username, password);
                var payload = JsonSerializer.Serialize(request);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(baseUrl, content);

                return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
            })
            .WithWarmUpDuration(TimeSpan.FromSeconds(5))
            .WithLoadSimulations(Simulation.KeepConstant(copies: 5, during: TimeSpan.FromSeconds(5)));
        }

        public NBomber.Contracts.ScenarioProps CreateLoginScenario(HttpClient httpClient)
        {
            return Scenario.Create("LoginEndpoint", async context =>
            {
                var baseUrl = "http://localhost:80/account/login";

                var username = $"user_{new Random().Next(1, int.MaxValue)}";
                var password = $"pass_{new Random().Next(1, int.MaxValue)}";

                var request = new LoginRequest(username, password);
                var payload = JsonSerializer.Serialize(request);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(baseUrl, content);

                return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
            })
            .WithWarmUpDuration(TimeSpan.FromSeconds(5))
            .WithLoadSimulations(Simulation.KeepConstant(copies: 5, during: TimeSpan.FromSeconds(5)));
        }

    }
}
