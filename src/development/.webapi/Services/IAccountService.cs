
using AccountAPI.Models;

namespace AccountAPI.Services;

public interface IAccountService
{
    Task<bool> RegisterAsync(User user);
    Task<string?> LoginAsync(User user);
    Task<string?> ValidateSessionAsync(string sessionId);
    Task<bool> DeleteUserAsync(string username);
}
