using System.Net.Http;
using Xunit;
using AccountAPI.Tests.T2.Framework.Helpers;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;

namespace AccountAPI.Tests.T2.Framework
{
    public abstract class TestBase : IClassFixture<TestFixture>
    {
        protected readonly HttpClient _client;
        protected readonly WebHelper _webHelper;
        protected readonly UserHelper _userHelper;

        protected TestBase(TestFixture fixture)
        {
            _client = fixture.Client;
            _webHelper = new WebHelper(_client);
            _userHelper = new UserHelper();
        }
    }

    public class TestFixture : IAsyncLifetime
    {
        public HttpClient Client { get; private set; }
        private const string ContainerName = "whoami-webapi-test";
        private const string ImageName = "whoami-webapi";
        private const string ContainerPort = "8080";
        private string? _dynamicHostPort;

        public TestFixture()
        {
            Client = new HttpClient();
        }

        public async Task InitializeAsync()
        {
            // Ensure the container is not running from a previous failed test run
            await StopAndRemoveContainer();

            // Start the container if it's not running
            if (!await IsContainerRunning())
            {
                await StartContainer();
                await Task.Delay(2000); // Give container a moment to start its process
                await WaitForApiReady(); // Wait for the API to be ready
            }
        }

        public async Task DisposeAsync()
        {
            Client.Dispose();
            await StopAndRemoveContainer();
        }

        private async Task<bool> IsContainerRunning()
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = $"ps -f name={ContainerName} --format \"{{.Names}}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();
            return output.Trim() == ContainerName;
        }

        private async Task StartContainer()
        {
            string arguments = $"run -d --name {ContainerName} -p {ContainerPort} -e ASPNETCORE_ENVIRONMENT=Development {ImageName}";
            Console.WriteLine($"Executing docker command: docker {arguments}");
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true, // Capture stderr
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string stdout = await process.StandardOutput.ReadToEndAsync();
            string stderr = await process.StandardError.ReadToEndAsync(); // Read stderr
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Docker Start Failed with exit code {process.ExitCode}. Error: {stderr}");
            }

            if (!string.IsNullOrEmpty(stderr))
            {
                Console.WriteLine($"Docker Start Stderr: {stderr}");
            }

            // Get the dynamically assigned port
            var portProcess = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = $"port {ContainerName} {ContainerPort}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            portProcess.Start();
            string portOutput = await portProcess.StandardOutput.ReadToEndAsync();
            await portProcess.WaitForExitAsync();

            var match = Regex.Match(portOutput, @"0.0.0.0:(\d+)");
            if (match.Success)
            {
                _dynamicHostPort = match.Groups[1].Value;
                Client.BaseAddress = new System.Uri($"http://localhost:{_dynamicHostPort}/");
                Console.WriteLine($"API will be accessible at: {Client.BaseAddress}");
            }
            else
            {
                throw new Exception($"Could not determine dynamic host port. Docker port output: {portOutput}");
            }
        }

        private async Task StopAndRemoveContainer()
        {
            Console.WriteLine($"Attempting to stop and remove container: {ContainerName}");
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = $"rm -f {ContainerName}", // Force remove
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string stderr = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0 && !stderr.Contains($"No such container: {ContainerName}"))
            {
                Console.WriteLine($"Docker RM -f Failed with exit code {process.ExitCode}. Error: {stderr}");
                throw new Exception($"Docker RM -f Failed");
            }
            Console.WriteLine($"Container {ContainerName} stopped and removed.");
        }

        private async Task WaitForApiReady()
        {
            int maxRetries = 30; // Increased retries
            int retryDelayMs = 2000; // Increased delay

            Console.WriteLine("Waiting for API to become ready...");
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    var response = await Client.GetAsync("swagger/v1/swagger.json"); // Check Swagger JSON endpoint
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("API is ready!");
                        return;
                    }
                    else
                    {
                        Console.WriteLine($"Attempt {i + 1}/{maxRetries}: API returned status code {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Attempt {i + 1}/{maxRetries}: Connection failed. {ex.Message}");
                }
                await Task.Delay(retryDelayMs);
            }
            throw new Exception("API did not become ready within the expected time.");
        }
    }
}