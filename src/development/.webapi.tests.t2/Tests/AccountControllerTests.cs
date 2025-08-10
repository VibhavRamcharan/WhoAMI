using AccountAPI.Tests.T2.Framework;
using AccountAPI.Tests.T2.Framework.Helpers;
using Newtonsoft.Json.Linq;

namespace AccountAPI.Tests.T2.Tests
{
    public class AccountControllerTests : ControllerBase
    {
        public AccountControllerTests() : base()
        {
        }

        [Fact]
        [Trait("Category", TestCategories.FunctionalTests)]
        public async Task GIVEN_UserDoesNotExist_WHEN_Register_THEN_ReturnOk()
        {
            // Arrange
            var user = _userHelper.GenerateNewUserCredentials();

            // Act
            var response = await _webHelper.CreateUserAsync(user.Username, user.Password);
            var responseContent = await _webHelper.GetResponseContentAsync(response);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("User registered successfully.", responseContent);

            // Cleanup
            await _webHelper.DeleteUserAsync(user.Username);
        }

        [Fact]
        [Trait("Category", TestCategories.FunctionalTests)]
        public async Task GIVEN_UserExists_WHEN_Register_THEN_ReturnConflict()
        {
            // Arrange
            var user = _userHelper.GenerateNewUserCredentials();
            await _webHelper.CreateUserAsync(user.Username, user.Password);

            // Act
            var response = await _webHelper.CreateUserAsync(user.Username, user.Password);
            var responseContent = await _webHelper.GetResponseContentAsync(response);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Conflict, response.StatusCode);
            Assert.Equal("Username already exists.", responseContent);

            // Cleanup
            await _webHelper.DeleteUserAsync(user.Username);
        }

        [Fact]
        [Trait("Category", TestCategories.FunctionalTests)]
        public async Task GIVEN_EmptyUsername_WHEN_Register_THEN_ReturnConflict()
        {
            // Arrange
            var user = _userHelper.GenerateNewUserCredentials();
            user.Username = "";

            // Act
            var response = await _webHelper.CreateUserAsync(user.Username, user.Password);
            var responseContent = await _webHelper.GetResponseContentAsync(response);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Conflict, response.StatusCode);
            Assert.Equal("Username cannot be empty.", responseContent);
        }

        [Fact]
        [Trait("Category", TestCategories.FunctionalTests)]
        public async Task GIVEN_EmptyPassword_WHEN_Register_THEN_ReturnConflict()
        {
            // Arrange
            var user = _userHelper.GenerateNewUserCredentials();
            user.Password = "";

            // Act
            var response = await _webHelper.CreateUserAsync(user.Username, user.Password);
            var responseContent = await _webHelper.GetResponseContentAsync(response);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Conflict, response.StatusCode);
            Assert.Equal("Password cannot be empty.", responseContent);
        }

        [Fact]
        [Trait("Category", TestCategories.FunctionalTests)]
        public async Task GIVEN_InvalidCredentials_WHEN_Login_THEN_ReturnUnauthorized()
        {
            // Arrange
            var user = _userHelper.GenerateNewUserCredentials();
            await _webHelper.CreateUserAsync(user.Username, user.Password);
            var loginUser = new Models.User { Username = user.Username, Password = "wrongpassword" };

            // Act
            var response = await _webHelper.PostAsync("Account/login", loginUser);
            var responseContent = await _webHelper.GetResponseContentAsync(response);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("Invalid username or password.", responseContent);

            // Cleanup
            await _webHelper.DeleteUserAsync(user.Username);
        }

        [Fact]
        [Trait("Category", TestCategories.FunctionalTests)]
        public async Task GIVEN_CorrectCredentials_WHEN_Login_THEN_ReturnOk()
        {
            // Arrange
            var user = _userHelper.GenerateNewUserCredentials();
            await _webHelper.CreateUserAsync(user.Username, user.Password);

            // Act
            var response = await _webHelper.PostAsync("Account/login", user);
            var responseContent = await _webHelper.GetResponseContentAsync(response);
            var jsonResponse = JObject.Parse(responseContent);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(jsonResponse["sessionId"]);

            // Cleanup
            await _webHelper.DeleteUserAsync(user.Username);
        }

        [Fact]
        [Trait("Category", TestCategories.FunctionalTests)]
        public async Task GIVEN_InvalidSession_WHEN_ValidateSession_THEN_ReturnUnauthorized()
        {
            // Arrange
            var sessionId = Guid.NewGuid().ToString();

            // Act
            var response = await _webHelper.GetAsync($"Account/validatesession?sessionId={sessionId}");
            var responseContent = await _webHelper.GetResponseContentAsync(response);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("Invalid session.", responseContent);
        }

        [Fact]
        [Trait("Category", TestCategories.FunctionalTests)]
        public async Task GIVEN_ValidSession_WHEN_ValidateSession_THEN_ReturnOk()
        {
            // Arrange
            var user = _userHelper.GenerateNewUserCredentials();
            await _webHelper.CreateUserAsync(user.Username, user.Password);
            var loginResponse = await _webHelper.PostAsync("Account/login", user);
            var loginContent = await _webHelper.GetResponseContentAsync(loginResponse);
            var loginJson = JObject.Parse(loginContent);
            var sessionId = loginJson["sessionId"]?.ToString();

            // Act
            var response = await _webHelper.GetAsync($"Account/validatesession?sessionId={sessionId}");
            var responseContent = await _webHelper.GetResponseContentAsync(response);
            var jsonResponse = JObject.Parse(responseContent);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(user.Username, jsonResponse["username"]?.ToString());

            // Cleanup
            await _webHelper.DeleteUserAsync(user.Username);
        }

        [Fact]
        [Trait("Category", TestCategories.FunctionalTests)]
        public async Task GIVEN_UserExists_WHEN_DeleteUser_THEN_ReturnOk()
        {
            // Arrange
            var user = _userHelper.GenerateNewUserCredentials();
            await _webHelper.CreateUserAsync(user.Username, user.Password);

            // Act
            var response = await _webHelper.DeleteUserAsync(user.Username);
            var responseContent = await _webHelper.GetResponseContentAsync(response);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("User deleted successfully.", responseContent);
        }

        [Fact]
        [Trait("Category", TestCategories.FunctionalTests)]
        public async Task GIVEN_UserDoesNotExist_WHEN_DeleteUser_THEN_ReturnNotFound()
        {
            // Arrange
            var username = "nonexistentuser";

            // Act
            var response = await _webHelper.DeleteUserAsync(username);
            var responseContent = await _webHelper.GetResponseContentAsync(response);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("User not found.", responseContent);
        }

        [Fact]
        [Trait("Category", TestCategories.FunctionalTests)]
        public async Task GIVEN_EmptyUsername_WHEN_Login_THEN_ReturnUnauthorized()
        {
            // Arrange
            var user = _userHelper.GenerateNewUserCredentials();
            user.Username = "";

            // Act
            var response = await _webHelper.PostAsync("Account/login", user);
            var responseContent = await _webHelper.GetResponseContentAsync(response);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("Invalid username or password.", responseContent);
        }

        [Fact]
        [Trait("Category", TestCategories.FunctionalTests)]
        public async Task GIVEN_EmptyPassword_WHEN_Login_THEN_ReturnUnauthorized()
        {
            // Arrange
            var user = _userHelper.GenerateNewUserCredentials();
            user.Password = "";

            // Act
            var response = await _webHelper.PostAsync("Account/login", user);
            var responseContent = await _webHelper.GetResponseContentAsync(response);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("Invalid username or password.", responseContent);
        }

        [Fact]
        [Trait("Category", TestCategories.FunctionalTests)]
        public async Task GIVEN_EmptySessionId_WHEN_ValidateSession_THEN_ReturnUnauthorized()
        {
            // Arrange
            var sessionId = "";

            // Act
            var response = await _webHelper.GetAsync($"Account/validatesession?sessionId={sessionId}");
            var responseContent = await _webHelper.GetResponseContentAsync(response);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            JObject jsonResponse = JObject.Parse(responseContent);
            Assert.Equal("One or more validation errors occurred.", jsonResponse["title"]?.ToString());
        }
    }
}
