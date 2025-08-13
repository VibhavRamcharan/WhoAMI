using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AccountAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var hostName = Dns.GetHostName();
            var ipAddress = Dns.GetHostEntry(hostName).AddressList.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            var port = HttpContext.Connection.LocalPort;

            var healthInfo = new
            {
                Status = "Healthy",
                Application = "AccountAPI",
                HostName = hostName,
                IpAddress = ipAddress?.ToString(),
                Port = port,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
            };

            _logger.LogInformation("Health check requested. Returning health information.");
            return Ok(healthInfo);
        }
    }
}
