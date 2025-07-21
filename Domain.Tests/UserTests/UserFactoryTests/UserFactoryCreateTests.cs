using Domain.Factory;
using Domain.IRepository;
using Domain.Visitor;
using Moq;

namespace Domain.Tests.UserTests.UserFactoryTests;

public class UserFactoryCreateTests
{
    [Fact]
    public async Task Create_WithExistingUserId_ShouldReturnNull()
    {
        // arrange
        var existingId = Guid.NewGuid();
        var userRepoDouble = new Mock<IUserRepository>();
        userRepoDouble.Setup(repo => repo.Exists(existingId)).ReturnsAsync(true);

        var factory = new UserFactory(userRepoDouble.Object);

        // act
        var result = await factory.Create(existingId);

        // assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Create_WithNonExistingUserId_ShouldReturnUser()
    {
        // arrange
        var newId = Guid.NewGuid();
        var userRepoDouble = new Mock<IUserRepository>();
        userRepoDouble.Setup(repo => repo.Exists(newId)).ReturnsAsync(false);

        var factory = new UserFactory(userRepoDouble.Object);

        // act
        var result = await factory.Create(newId);

        // assert
        Assert.NotNull(result);
        Assert.Equal(newId, result.Id);
    }

    [Fact]
    public void Create_WithUserVisitor_ShouldReturnUser()
    {
        // arrange
        var visitorId = Guid.NewGuid();
        var userVisitorMock = new Mock<IUserVisitor>();
        userVisitorMock.Setup(v => v.Id).Returns(visitorId);

        var factory = new UserFactory(Mock.Of<IUserRepository>());

        // act
        var user = factory.Create(userVisitorMock.Object);

        // assert
        Assert.NotNull(user);
        Assert.Equal(visitorId, user.Id);
    }
}
