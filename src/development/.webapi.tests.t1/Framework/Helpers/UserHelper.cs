using AccountAPI.Models;

namespace AccountAPI.Tests.T1.Framework.Helpers
{
    internal class SessionHelper
    {
        internal Guid GenerateRandomSessionId()
        {
            return Guid.NewGuid();
        }
    }
    internal class UserHelper
    {
        private readonly Random _random = new Random();

        internal User GenerateNewUserCredentials()
        {
            var randomNumber = _random.Next(1000, 9999);
            return new User
            {
                Username = $"Test{randomNumber}",
                Password = "Test1234$"
            };
        }
    }
}
