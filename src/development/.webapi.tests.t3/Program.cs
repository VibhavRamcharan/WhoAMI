using AccountAPI.Tests.T3.Scenarios;
using NBomber.CSharp;

namespace AccountAPI.Tests.T3
{
    class Program
    {  
        static async Task Main(string[] args)
        {
            using var httpClient = new HttpClient();
            
            var controller = new AccountControllerScenarios();

            var registerScenario = controller.CreateRegisterScenario(httpClient);

            var loginEndpoint = controller.CreateLoginScenario(httpClient);

            NBomberRunner
                .RegisterScenarios(registerScenario, loginEndpoint)
                .Run();
        }
    }
}