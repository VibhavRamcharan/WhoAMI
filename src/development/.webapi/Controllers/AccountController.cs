
using AccountAPI.Models;
using AccountAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccountAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (string.IsNullOrEmpty(user.Username))
        {
            return Conflict("Username cannot be empty.");
        }
        if (string.IsNullOrEmpty(user.Password))
        {
            return Conflict("Password cannot be empty.");
        }

        if (!await _accountService.RegisterAsync(user))
        {
            return Conflict("Username already exists.");
        }

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        var sessionId = await _accountService.LoginAsync(user);
        if (sessionId == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        return Ok(new { SessionId = sessionId });
    }

    [HttpGet("validatesession")]
    public async Task<IActionResult> ValidateSession([FromQuery] string sessionId)
    {
        var username = await _accountService.ValidateSessionAsync(sessionId);
        if (username == null)
        {
            return Unauthorized("Invalid session.");
        }

        return Ok(new { Username = username });
    }

    [HttpDelete("deleteuser")]
    public async Task<IActionResult> DeleteUser([FromQuery] string username)
    {
        if (!await _accountService.DeleteUserAsync(username))
        {
            return NotFound("User not found.");
        }

        return Ok("User deleted successfully.");
    }
}
