using NBomber.CSharp;
using NBomber.Http.CSharp;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AccountAPI.Tests.T3.Framework;
using AccountAPI.Tests.T3.Models;

namespace AccountAPI.Tests.T3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var fixture = new TestFixture();
            await fixture.InitializeAsync();

            var httpClient = fixture.Client;
            var userApiClient = new UserApiClient(httpClient);

            // Register a user first
            var registerUser = new User { Username = "testuser", Password = "password" };
            var registerResponse = await userApiClient.RegisterUser(registerUser);

            if (registerResponse.Payload.IsSome() && !registerResponse.Payload.Value.IsSuccessStatusCode)
            {
                Console.WriteLine($"User registration failed: {registerResponse.Payload}");
                fixture.Dispose();
                return;
            }

            var scenario = Scenario.Create("login_scenario", async context =>
            {
                var loginUser = new User { Username = "testuser", Password = "password" };
                var response = await userApiClient.LoginUser(loginUser);

                return response;
            })
            .WithLoadSimulations(new[] 
            {
                Simulation.KeepConstant(copies: 1, during: TimeSpan.FromSeconds(30))
            });

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();

            fixture.Dispose();
        }
    }
}