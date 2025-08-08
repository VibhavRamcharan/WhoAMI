using AccountAPI.Datastore;
using AccountAPI.Models;
using LiteDB;
using NUnit.Framework;
using System.IO;

namespace AccountAPI.Tests.Unit.DataStore;

[TestFixture]
public class AccountDataStoreTests
{
    private IAccountDataStore _sut;
    private LiteDatabase _liteDb;

    [SetUp]
    public void SetUp()
    {
        _liteDb = new LiteDatabase(new MemoryStream());
        _sut = new AccountDataStore(_liteDb);
    }

    [TearDown]
    public void TearDown()
    {
        _liteDb.Dispose();
    }

    [Test]
    public async Task CreateUserAsync_ShouldAddUserToDatabase()
    {
        // Arrange
        var user = new User { Username = "testuser", Password = "password" };

        // Act
        await _sut.CreateUserAsync(user);

        // Assert
        var result = await _sut.GetUserAsync("testuser");
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo("testuser"));
    }

    [Test]
    public async Task GetUserAsync_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        var user = new User { Username = "testuser", Password = "password" };
        await _sut.CreateUserAsync(user);

        // Act
        var result = await _sut.GetUserAsync("testuser");

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo("testuser"));
    }

    [Test]
    public async Task DeleteUserAsync_WhenUserExists_ShouldDeleteUser()
    {
        // Arrange
        var user = new User { Username = "testuser", Password = "password" };
        await _sut.CreateUserAsync(user);

        // Act
        await _sut.DeleteUserAsync("testuser");

        // Assert
        var result = await _sut.GetUserAsync("testuser");
        Assert.That(result, Is.Null);
    }
}
