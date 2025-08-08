using AccountAPI.Tests.T2.Models;
using System;

namespace AccountAPI.Tests.T2.Framework.Helpers
{
    public class UserHelper
    {
        private readonly Random _random = new Random();

        public User GenerateNewUserCredentials()
        {
            var randomNumber = _random.Next(1000, 9999);
            return new User
            {
                Username = $"TestUser{randomNumber}",
                Password = "TestPassword123$"
            };
        }
    }
}
