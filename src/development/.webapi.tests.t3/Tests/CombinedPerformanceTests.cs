using NBomber.CSharp;
using NBomber.Http.CSharp;
using NBomber.Contracts;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AccountAPI.Tests.T3.Framework;
using AccountAPI.Tests.T3.Models;
using AccountAPI.Tests.T3.Framework.Helpers;
using System.Linq;

namespace AccountAPI.Tests.T3.Tests
{
    public class CombinedPerformanceTests
    {
        private readonly HttpClient _httpClient;
        private readonly UserApiClient _userApiClient;

        public CombinedPerformanceTests(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _userApiClient = new UserApiClient(_httpClient);
        }

        public ScenarioProps RegisterScenario => Scenario.Create("register_scenario", async context =>
        {
            var registerUser = UserHelper.GenerateRandomUser();
            var response = await _userApiClient.RegisterUser(registerUser);

            return response;
        })
        .WithLoadSimulations(new[]
        {
            Simulation.KeepConstant(copies: 1, during: TimeSpan.FromSeconds(30))
        });

        public ScenarioProps GetLoginScenario(User user) => Scenario.Create("login_scenario", async context =>
        {
            var loginUser = new User { Username = user.Username, Password = user.Password };
            var response = await _userApiClient.LoginUser(loginUser);

            return response;
        })
        .WithLoadSimulations(new[]
        {
            Simulation.KeepConstant(copies: 1, during: TimeSpan.FromSeconds(30))
        });

                public async Task RunAllScenarios()
        {
            // Register a user for the login scenario
            var loginUser = UserHelper.GenerateRandomUser();
            var registerResponse = await _userApiClient.RegisterUser(loginUser);

            if (registerResponse.Payload.IsSome() && !registerResponse.Payload.Value.IsSuccessStatusCode)
            {
                Console.WriteLine($"User registration failed for login scenario: {registerResponse.Payload}");
                throw new Exception("User registration failed, cannot proceed with performance test.");
            }

            var scenarioStats = NBomberRunner
                .RegisterScenarios(RegisterScenario, GetLoginScenario(loginUser))
                .Run();

            // Assertions for Register Scenario
            var registerScenarioStats = scenarioStats.ScenarioStats.FirstOrDefault(x => x.ScenarioName == "register_scenario");
            if (registerScenarioStats == null)
            {
                throw new Exception("Register scenario statistics not found.");
            }
            const double expectedRegisterRps = 4.0;
            if (registerScenarioStats.Ok.Request.RPS < expectedRegisterRps)
            {
                throw new Exception($"Register scenario RPS ({registerScenarioStats.Ok.Request.RPS}) is below the required {expectedRegisterRps} RPS.");
            }
            Console.WriteLine($"Register scenario RPS ({registerScenarioStats.Ok.Request.RPS}) meets the required {expectedRegisterRps} RPS.");
            if (registerScenarioStats.Fail.Request.Count > 0)
            {
                throw new Exception($"Register scenario has failed requests: {registerScenarioStats.Fail.Request.Count}.");
            }
            Console.WriteLine($"Register scenario has no failed requests.");

            // Assertions for Login Scenario
            var loginScenarioStats = scenarioStats.ScenarioStats.FirstOrDefault(x => x.ScenarioName == "login_scenario");
            if (loginScenarioStats == null)
            {
                throw new Exception("Login scenario statistics not found.");
            }
            const double expectedLoginRps = 4.0;
            if (loginScenarioStats.Ok.Request.RPS < expectedLoginRps)
            {
                throw new Exception($"Login scenario RPS ({loginScenarioStats.Ok.Request.RPS}) is below the required {expectedLoginRps} RPS.");
            }
            Console.WriteLine($"Login scenario RPS ({loginScenarioStats.Ok.Request.RPS}) meets the required {expectedLoginRps} RPS.");
            if (loginScenarioStats.Fail.Request.Count > 0)
            {
                throw new Exception($"Login scenario has failed requests: {loginScenarioStats.Fail.Request.Count}.");
            }
            Console.WriteLine($"Login scenario has no failed requests.");
        }
    }
}