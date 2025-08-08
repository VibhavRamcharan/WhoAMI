using AccountAPI.Datastore;
using AccountAPI.Models;
using AccountAPI.Services;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using NUnit.Framework;

namespace AccountAPI.Tests.Unit.Services;

[TestFixture]
public class AccountServiceTests
{
    private IAccountService _sut;
    private IAccountDataStore _accountDataStore;
    private IMemoryCache _memoryCache;
    private IEncryptionService _encryptionService;

    [SetUp]
    public void SetUp()
    {
        _accountDataStore = Substitute.For<IAccountDataStore>();
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _encryptionService = Substitute.For<IEncryptionService>();
        _sut = new AccountService(_accountDataStore, _memoryCache, _encryptionService);
    }

    [TearDown]
    public void TearDown()
    {
        _memoryCache.Dispose();
    }

    [Test]
    public async Task RegisterAsync_WhenUserDoesNotExist_ReturnsTrue()
    {
        // Arrange
        var user = new User { Username = "newuser", Password = "password" };
        _accountDataStore.UserExistsAsync(user.Username).Returns(false);
        _encryptionService.HashPassword(user.Password).Returns("hashedpassword");

        // Act
        var result = await _sut.RegisterAsync(user);

        // Assert
        Assert.That(result, Is.True);
        await _accountDataStore.Received(1).CreateUserAsync(Arg.Is<User>(u => u.Password == "hashedpassword"));
    }

    [Test]
    public async Task RegisterAsync_WhenUserExists_ReturnsFalse()
    {
        // Arrange
        var user = new User { Username = "existinguser", Password = "password" };
        _accountDataStore.UserExistsAsync(user.Username).Returns(true);

        // Act
        var result = await _sut.RegisterAsync(user);

        // Assert
        Assert.That(result, Is.False);
        await _accountDataStore.DidNotReceive().CreateUserAsync(Arg.Any<User>());
    }

    [Test]
    public async Task LoginAsync_WithValidCredentials_ReturnsSessionId()
    {
        // Arrange
        var user = new User { Username = "testuser", Password = "password" };
        _accountDataStore.GetUserAsync(user.Username).Returns(Task.FromResult(new User { Username = "testuser", Password = "hashedpassword" }));
        _encryptionService.VerifyPassword(user.Password, "hashedpassword").Returns(true);

        // Act
        var sessionId = await _sut.LoginAsync(user);

        // Assert
        Assert.That(sessionId, Is.Not.Null);
        var cachedUsername = _memoryCache.Get(sessionId);
        Assert.That(cachedUsername, Is.EqualTo(user.Username));
    }

    [Test]
    public async Task LoginAsync_WithInvalidCredentials_ReturnsNull()
    {
        // Arrange
        var user = new User { Username = "testuser", Password = "wrongpassword" };
        _accountDataStore.GetUserAsync(user.Username).Returns(Task.FromResult(new User { Username = "testuser", Password = "hashedpassword" }));
        _encryptionService.VerifyPassword(user.Password, "hashedpassword").Returns(false);

        // Act
        var sessionId = await _sut.LoginAsync(user);

        // Assert
        Assert.That(sessionId, Is.Null);
    }

    [Test]
    public async Task ValidateSessionAsync_WithValidSession_ReturnsUsername()
    {
        // Arrange
        var sessionId = "validsession";
        var username = "testuser";
        _memoryCache.Set(sessionId, username);

        // Act
        var result = await _sut.ValidateSessionAsync(sessionId);

        // Assert
        Assert.That(result, Is.EqualTo(username));
    }

    [Test]
    public async Task ValidateSessionAsync_WithInvalidSession_ReturnsNull()
    {
        // Arrange
        var sessionId = "invalidsession";

        // Act
        var result = await _sut.ValidateSessionAsync(sessionId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task DeleteUserAsync_WhenUserExists_ReturnsTrue()
    {
        // Arrange
        var username = "existinguser";
        _accountDataStore.UserExistsAsync(username).Returns(true);
        _accountDataStore.DeleteUserAsync(username).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.DeleteUserAsync(username);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteUserAsync_WhenUserDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var username = "nonexistinguser";
        _accountDataStore.UserExistsAsync(username).Returns(false);

        // Act
        var result = await _sut.DeleteUserAsync(username);

        // Assert
        Assert.That(result, Is.False);
    }
}
