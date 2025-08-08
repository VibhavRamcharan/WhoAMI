using NBomber.CSharp;
using NBomber.Http.CSharp;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AccountAPI.Tests.T3.Framework;
using AccountAPI.Tests.T3.Models;
using AccountAPI.Tests.T3.Tests;

namespace AccountAPI.Tests.T3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var fixture = new TestFixture();
            await fixture.InitializeAsync();

            var httpClient = fixture.Client;

            // Run Register Performance Tests
            var registerPerformanceTests = new RegisterPerformanceTests(httpClient);
            await registerPerformanceTests.RunRegisterScenario();

            // Run Login Performance Tests
            var loginPerformanceTests = new LoginPerformanceTests(httpClient);
            await loginPerformanceTests.RunLoginScenario();

            fixture.Dispose();
        }
    }
}