using System;
using AccountAPI.Tests.T3.Models;

namespace AccountAPI.Tests.T3.Framework.Helpers
{
    public static class UserHelper
    {
        public static User GenerateRandomUser()
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
            var username = $"test{guid}@automation.com";
            var password = $"password{guid}";

            return new User { Username = username, Password = password };
        }
    }
}