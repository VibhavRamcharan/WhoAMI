using AccountAPI.Tests.T3.Framework;
using AccountAPI.Tests.T3.Tests;

namespace AccountAPI.Tests.T3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var fixture = new PerformanceFixture();
            await fixture.InitializeAsync();

            var httpClient = fixture.Client;

            var combinedTests = new CombinedPerformanceTests(httpClient);
            await combinedTests.RunAllScenarios();

            fixture.Dispose();
        }
    }
}