
namespace AccountAPI.Tests.T3.Framework
{
    public class PerformanceFixture
    {
        public HttpClient Client { get; private set; }

        public PerformanceFixture()
        {
            Client = new HttpClient { BaseAddress = new Uri("http://localhost") };
        }

        internal async Task InitializeAsync()
        {
            
        }

        internal void Dispose()
        {
            Client.Dispose();
        }
    }
}