using AccountAPI.Models;
using LiteDB;

namespace AccountAPI.Datastore;

using System.Threading.Tasks;

public class AccountDataStore : IAccountDataStore
{
    private readonly LiteDatabase _db;

    public AccountDataStore()
    {
        _db = new LiteDatabase(@"Account.db");
    }

    public AccountDataStore(LiteDatabase db)
    {
        _db = db;
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await Task.Run(() =>
        {
            var users = _db.GetCollection<User>("users");
            return users.Exists(x => x.Username == username);
        });
    }

    public async Task CreateUserAsync(User user)
    {
        await Task.Run(() =>
        {
            var users = _db.GetCollection<User>("users");
            users.Insert(user);
        });
    }

    public async Task<User> GetUserAsync(string username)
    {
        return await Task.Run(() =>
        {
            var users = _db.GetCollection<User>("users");
            return users.FindOne(x => x.Username == username);
        });
    }

    public async Task DeleteUserAsync(string username)
    {
        await Task.Run(() =>
        {
            var users = _db.GetCollection<User>("users");
            users.DeleteMany(x => x.Username == username);
        });
    }
}