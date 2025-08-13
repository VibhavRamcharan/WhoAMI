using System.Text;
using System.Text.Json;
using NBomber.CSharp;

namespace AccountAPI.Tests.T3.Scenarios
{
    internal class AccountControllerScenario
    {
        record RegisterRequest(string Username, string Password);

        internal NBomber.Contracts.ScenarioProps CreateRegisterScenario(HttpClient httpClient)
        {
            return Scenario.Create("CreateRegisterScenario", async context =>
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
        .WithLoadSimulations(Simulation.KeepConstant(copies: 5, during: TimeSpan.FromSeconds(15)));
        }
    }
}
