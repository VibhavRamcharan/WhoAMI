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
    public class LoginPerformanceTests
    {
        private readonly HttpClient _httpClient;
        private readonly UserApiClient _userApiClient;

        public LoginPerformanceTests(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _userApiClient = new UserApiClient(_httpClient);
        }

        public async Task RunLoginScenario()
        {
            // Register a user first
            var registerUser = new User { Username = "testuser", Password = "password" };
            var registerResponse = await _userApiClient.RegisterUser(registerUser);

            if (registerResponse.Payload.IsSome() && !registerResponse.Payload.Value.IsSuccessStatusCode)
            {
                Console.WriteLine($"User registration failed: {registerResponse.Payload}");
                throw new Exception("User registration failed, cannot proceed with performance test.");
            }

            var scenario = Scenario.Create("login_scenario", async context =>
            {
                var loginUser = new User { Username = "testuser", Password = "password" };
                var response = await _userApiClient.LoginUser(loginUser);

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
            var loginScenarioStats = scenarioStats.ScenarioStats.FirstOrDefault(x => x.ScenarioName == "login_scenario");

            if (loginScenarioStats == null)
            {
                throw new Exception("Login scenario statistics not found.");
            }

            // RPS Assertion
            const double expectedRps = 4.0;
            if (loginScenarioStats.Ok.Request.RPS < expectedRps)
            {
                throw new Exception($"Login scenario RPS ({loginScenarioStats.Ok.Request.RPS}) is below the required {expectedRps} RPS.");
            }
            Console.WriteLine($"Login scenario RPS ({loginScenarioStats.Ok.Request.RPS}) meets the required {expectedRps} RPS.");

            // Error Rate Assertion
            if (loginScenarioStats.Fail.Request.Count > 0)
            {
                throw new Exception($"Login scenario has failed requests: {loginScenarioStats.Fail.Request.Count}.");
            }
            Console.WriteLine($"Login scenario has no failed requests.");
        }
    }
}
