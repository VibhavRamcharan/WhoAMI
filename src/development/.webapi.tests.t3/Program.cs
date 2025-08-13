using AccountAPI.Tests.T3.Scenarios;
using NBomber.CSharp;

namespace AccountAPI.Tests.T3
{
    class Program
    {  
        static async Task Main(string[] args)
        { 
            var registerScenario = new AccountControllerScenario().CreateRegisterScenario(new HttpClient());

            NBomberRunner
                .RegisterScenarios(registerScenario)
                .Run();
        }
    }

}