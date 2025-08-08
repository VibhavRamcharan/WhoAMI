using AccountAPI.Datastore;
using AccountAPI.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace AccountAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly IMemoryCache _memoryCache;
        private readonly IEncryptionService _encryptionService;

        public AccountService(IAccountDataStore accountDataStore, IMemoryCache memoryCache, IEncryptionService encryptionService)
        {
            _accountDataStore = accountDataStore;
            _memoryCache = memoryCache;
            _encryptionService = encryptionService;
        }

        public async Task<bool> RegisterAsync(User user)
        {
            if (await _accountDataStore.UserExistsAsync(user.Username))
            {
                return false;
            }

            user.Password = _encryptionService.HashPassword(user.Password);
            await _accountDataStore.CreateUserAsync(user);
            return true;
        }

        public async Task<string?> LoginAsync(User user)
        {
            var storedUser = await _accountDataStore.GetUserAsync(user.Username!);
            if (storedUser == null || !_encryptionService.VerifyPassword(user.Password!, storedUser.Password!))
            {
                return null;
            }

            var sessionId = Guid.NewGuid().ToString();
            _memoryCache.Set(sessionId, storedUser.Username!, TimeSpan.FromHours(1));
            return sessionId;
        }

        public async Task<string?> ValidateSessionAsync(string sessionId)
        {
            if (_memoryCache.TryGetValue(sessionId, out string? username))
            {
                return await Task.FromResult(username);
            }

            return null;
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            if (!await _accountDataStore.UserExistsAsync(username))
            {
                return false;
            }

            await _accountDataStore.DeleteUserAsync(username);
            return true;
        }
    }
}
