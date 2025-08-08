using NBomber.CSharp;
using NBomber.Http.CSharp;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AccountAPI.Tests.T3.Framework;
using AccountAPI.Tests.T3.Models;
using System.Linq;

namespace AccountAPI.Tests.T3.Tests
{
    public class RegisterPerformanceTests
    {
        private readonly HttpClient _httpClient;
        private readonly UserApiClient _userApiClient;

        public RegisterPerformanceTests(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _userApiClient = new UserApiClient(_httpClient);
        }

        public async Task RunRegisterScenario()
        {
            var scenario = Scenario.Create("register_scenario", async context =>
            {
                var registerUser = new User { Username = Guid.NewGuid().ToString(), Password = "password" };
                var response = await _userApiClient.RegisterUser(registerUser);

                return response;
            })
            .WithLoadSimulations(new[] 
            {
                Simulation.KeepConstant(copies: 1, during: TimeSpan.FromSeconds(30))
            });

            var scenarioStats = NBomberRunner
                .RegisterScenarios(scenario)
                .Run();

            // Assertions
            var registerScenarioStats = scenarioStats.ScenarioStats.FirstOrDefault(x => x.ScenarioName == "register_scenario");

            if (registerScenarioStats == null)
            {
                throw new Exception("Register scenario statistics not found.");
            }

            // RPS Assertion (placeholder)
            const double expectedRps = 4.0;
            if (registerScenarioStats.Ok.Request.RPS < expectedRps)
            {
                throw new Exception($"Register scenario RPS ({registerScenarioStats.Ok.Request.RPS}) is below the required {expectedRps} RPS.");
            }
            Console.WriteLine($"Register scenario RPS ({registerScenarioStats.Ok.Request.RPS}) meets the required {expectedRps} RPS.");

            // Error Rate Assertion
            if (registerScenarioStats.Fail.Request.Count > 0)
            {
                throw new Exception($"Register scenario has failed requests: {registerScenarioStats.Fail.Request.Count}.");
            }
            Console.WriteLine($"Register scenario has no failed requests.");
        }
    }
}
