using AccountAPI.Controllers;
using AccountAPI.Models;
using AccountAPI.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace AccountAPI.Tests.Unit.Controllers;

[TestFixture]
public class AccountControllerTests
{
    private AccountController _sut;
    private IAccountService _accountService;

    [SetUp]
    public void SetUp()
    {
        _accountService = Substitute.For<IAccountService>();
        _sut = new AccountController(_accountService);
    }

    [Test]
    public async Task Register_WithValidUser_ReturnsOk()
    {
        // Arrange
        var user = new User { Username = "test", Password = "password" };
        _accountService.RegisterAsync(user).Returns(true);

        // Act
        var result = await _sut.Register(user) as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.EqualTo("User registered successfully."));
    }

    [Test]
    public async Task Register_WhenUsernameExists_ReturnsConflict()
    {
        // Arrange
        var user = new User { Username = "existinguser", Password = "password" };
        _accountService.RegisterAsync(user).Returns(false);

        // Act
        var result = await _sut.Register(user) as ConflictObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(409));
        Assert.That(result.Value, Is.EqualTo("Username already exists."));
    }

    [Test]
    public async Task Login_WithValidCredentials_ReturnsOkWithSessionId()
    {
        // Arrange
        var user = new User { Username = "test", Password = "password" };
        var sessionId = "newsessionid";
        _accountService.LoginAsync(user).Returns(sessionId);

        // Act
        var result = await _sut.Login(user) as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.GetType().GetProperty("SessionId")!.GetValue(result.Value), Is.EqualTo(sessionId));
    }

    [Test]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var user = new User { Username = "test", Password = "wrongpassword" };
        _accountService.LoginAsync(user).Returns((string?)null);

        // Act
        var result = await _sut.Login(user) as UnauthorizedObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(401));
        Assert.That(result.Value, Is.EqualTo("Invalid username or password."));
    }

    [Test]
    public async Task ValidateSession_WithValidSession_ReturnsOkWithUsername()
    {
        // Arrange
        var sessionId = "validsession";
        var username = "testuser";
        _accountService.ValidateSessionAsync(sessionId).Returns(username);

        // Act
        var result = await _sut.ValidateSession(sessionId) as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.GetType().GetProperty("Username")!.GetValue(result.Value), Is.EqualTo(username));
    }

    [Test]
    public async Task ValidateSession_WithInvalidSession_ReturnsUnauthorized()
    {
        // Arrange
        var sessionId = "invalidsession";
        _accountService.ValidateSessionAsync(sessionId).Returns((string?)null);

        // Act
        var result = await _sut.ValidateSession(sessionId) as UnauthorizedObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(401));
        Assert.That(result.Value, Is.EqualTo("Invalid session."));
    }

    [Test]
    public async Task DeleteUser_WhenUserExists_ReturnsOk()
    {
        // Arrange
        var username = "existinguser";
        _accountService.DeleteUserAsync(username).Returns(true);

        // Act
        var result = await _sut.DeleteUser(username) as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.EqualTo("User deleted successfully."));
    }

    [Test]
    public async Task DeleteUser_WhenUserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var username = "nonexistinguser";
        _accountService.DeleteUserAsync(username).Returns(false);

        // Act
        var result = await _sut.DeleteUser(username) as NotFoundObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(404));
        Assert.That(result.Value, Is.EqualTo("User not found."));
    }
}
