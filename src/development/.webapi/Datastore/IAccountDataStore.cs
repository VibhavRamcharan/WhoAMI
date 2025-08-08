
using AccountAPI.Models;

namespace AccountAPI.Datastore;

public interface IAccountDataStore
{
    Task<bool> UserExistsAsync(string username);
    Task CreateUserAsync(User user);
    Task<User> GetUserAsync(string username);
    Task DeleteUserAsync(string username);
}
