using AccountAPI.Controllers;
using AccountAPI.Datastore;
using AccountAPI.Models;
using AccountAPI.Services;
using AccountAPI.Tests.Framework.Builders;
using AccountAPI.Tests.Framework.Helpers;
using AccountAPI.Tests.Framework.Implements;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
namespace AccountAPI.Tests.Tests.T1;
internal class AccountControllerTests : FrameworkTestFixtureBase
{
    protected IMemoryCache _memoryCache = null!;
    protected LiteDatabase _liteDb = null!;
    protected IAccountDataStore _dataStore = null!;
    protected IAccountService _service = null!;
    protected IEncryptionService _encryptionService = null!;
    protected AccountController _controller = null!;
    [SetUp]
    public override void SetupTest()
    {
        // Create in-memory LiteDB
        var memStream = new MemoryStream();
        _liteDb = new LiteDatabase(memStream);
        // Use the testable datastore
        _dataStore = new AccountDataStore(_liteDb);
        _memoryCache = new MemoryCacheMockBuilder().Build();
        _encryptionService = new EncryptionService();
        _service = new AccountService(_dataStore, _memoryCache, _encryptionService);
        _controller = new AccountController(_service);
    }
    [TearDown]
    public override void TearDownTest()
    {
        _liteDb?.Dispose();
    }
    [Test]
    [Category(TestCategories.RegressionTests)]
    public async Task GIVEN_UserExists_WHEN_Register_THEN_ReturnConflict()
    {
        // ARRANGE
        User user = UserHelper.GenerateNewUserCredentials();
        await _controller.Register(user);

        // ACT
        var result = await _controller.Register(user) as ConflictObjectResult;

        // ASSERT
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo("Username already exists."));
    }
    [Test]
    [Category(TestCategories.RegressionTests)]
    public async Task GIVEN_UserDoesNotExist_WHEN_Register_THEN_ReturnOk()
    {
        // ARRANGE
        User user = UserHelper.GenerateNewUserCredentials();

        // ACT
        var result = await _controller.Register(user) as OkObjectResult;

        // ASSERT
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo("User registered successfully."));
    }
    [Test]
    [Category(TestCategories.FunctionalTests)]
    public async Task GIVEN_InvalidCredentials_WHEN_Login_THEN_ReturnUnauthorized()
    {
        // ARRANGE
        User user = UserHelper.GenerateNewUserCredentials();
        string plainTextPassword = user.Password;
        await _controller.Register(user);
        user.Password = "wrongPassword";

        // ACT
        var result = await _controller.Login(user) as UnauthorizedObjectResult;

        // ASSERT
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo("Invalid username or password."));
    }
    [Test]
    [Category(TestCategories.FunctionalTests)]
    public async Task GIVEN_CorrectCredentials_WHEN_Login_THEN_ReturnOk()
    {
        // ARRANGE
        User user = UserHelper.GenerateNewUserCredentials();
        string plainTextPassword = user.Password;
        await _controller.Register(user);

        // ACT
        user.Password = plainTextPassword;
        var result = await _controller.Login(user) as OkObjectResult;

        // ASSERT
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.InstanceOf<object>());
    }
    [Test]
    [Category(TestCategories.FunctionalTests)]
    public async Task GIVEN_InvalidSession_WHEN_ValidateSession_THEN_ReturnUnauthorized()
    {
        // ARRANGE
        var sessionId = SessionHelper.GenerateRandomSessionId().ToString();

        // ACT
        var result = await _controller.ValidateSession(sessionId) as UnauthorizedObjectResult;

        // ASSERT
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo("Invalid session."));
    }
    [Test]
    [Category(TestCategories.FunctionalTests)]
    public async Task GIVEN_ValidSession_WHEN_ValidateSession_THEN_ReturnOk()
    {
        // ARRANGE
        var user = UserHelper.GenerateNewUserCredentials();        
        string mockSessionId = Guid.NewGuid().ToString();
        _memoryCache = new MemoryCacheMockBuilder()
            .WithTryGetValue(mockSessionId, user.Username, true)
            .Build();
        _service = new AccountService(_dataStore, _memoryCache, _encryptionService);
        _controller = new AccountController(_service);

        // ACT
        var result = await _controller.ValidateSession(mockSessionId) as OkObjectResult;

        // ASSERT
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.InstanceOf<object>());
    }
    [Test]
    [Category(TestCategories.FunctionalTests)]
    public async Task GIVEN_UserExists_WHEN_DeleteUser_THEN_ReturnOk()
    {
        // ARRANGE
        User user = UserHelper.GenerateNewUserCredentials();
        await _controller.Register(user);

        // ACT
        var result = await _controller.DeleteUser(user.Username) as OkObjectResult;

        // ASSERT
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo("User deleted successfully."));
    }
    [Test]
    [Category(TestCategories.FunctionalTests)]
    public async Task GIVEN_UserDoesNotExist_WHEN_DeleteUser_THEN_ReturnNotFound()
    {
        // ARRANGE
        string nonExistentUsername = "nonexistentuser";

        // ACT
        var result = await _controller.DeleteUser(nonExistentUsername) as NotFoundObjectResult;

        // ASSERT
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo("User not found."));
    }
    [Test]
    [Category(TestCategories.FunctionalTests)]
    public async Task GIVEN_EmptyUsername_WHEN_Register_THEN_ReturnConflict()
    {
        // ARRANGE
        User user = UserHelper.GenerateNewUserCredentials();
        user.Username = "";

        // ACT
        var result = await _controller.Register(user) as ConflictObjectResult;

        // ASSERT
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo("Username cannot be empty."));
    }
    [Test]
    [Category(TestCategories.FunctionalTests)]
    public async Task GIVEN_EmptyPassword_WHEN_Register_THEN_ReturnConflict()
    {
        // ARRANGE
        User user = UserHelper.GenerateNewUserCredentials();
        user.Password = "";

        // ACT
        var result = await _controller.Register(user) as ConflictObjectResult;

        // ASSERT
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo("Password cannot be empty."));
    }
    [Test]
    [Category(TestCategories.FunctionalTests)]
    public async Task GIVEN_EmptyUsername_WHEN_Login_THEN_ReturnUnauthorized()
    {
        // ARRANGE
        User user = UserHelper.GenerateNewUserCredentials();
        user.Username = "";

        // ACT
        var result = await _controller.Login(user) as UnauthorizedObjectResult;

        // ASSERT
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo("Invalid username or password."));
    }
    [Test]
    [Category(TestCategories.FunctionalTests)]
    public async Task GIVEN_EmptyPassword_WHEN_Login_THEN_ReturnUnauthorized()
    {
        // ARRANGE
        User user = UserHelper.GenerateNewUserCredentials();
        user.Password = "";

        // ACT
        var result = await _controller.Login(user) as UnauthorizedObjectResult;

        // ASSERT
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo("Invalid username or password."));
    }
    [Test]
    [Category(TestCategories.FunctionalTests)]
    public async Task GIVEN_EmptySessionId_WHEN_ValidateSession_THEN_ReturnUnauthorized()
    {
        // ARRANGE
        string emptySessionId = "";

        // ACT
        var result = await _controller.ValidateSession(emptySessionId) as UnauthorizedObjectResult;

        // ASSERT
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo("Invalid session."));
    }
}