using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.Visitor;
using Moq;

namespace Domain.Tests.CollaboratorTests.CollaboratorFactoryTests;

public class CollaboratorFactoryCreateTests
{
    [Fact]
    public async Task CollaboratorFactory_CreateWithUserIdAndPeriodDateTime_ShouldCreateNewCollaborator()
    {
        // arrange
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(initDate: DateTime.Today.AddDays(1), finalDate: DateTime.Today.AddDays(5));

        var userRepoDouble = new Mock<IUserRepository>();
        userRepoDouble.Setup(ur => ur.Exists(userId)).ReturnsAsync(true);

        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        collabRepoDouble.Setup(cr => cr.ExistsByUserIdAsync(userId)).ReturnsAsync(false);

        var factory = new CollaboratorFactory(collabRepoDouble.Object, userRepoDouble.Object);

        // act
        var result = await factory.Create(userId, period);

        // assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(period, result.PeriodDateTime);
    }

    [Fact]
    public async Task CollaboratorFactory_CreateWithNonExistingUserIdAndPeriodDateTime_ShouldNotCreateNewCollaborator()
    {
        // arrange
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(initDate: DateTime.Today.AddDays(1), finalDate: DateTime.Today.AddDays(5));

        var userRepoDouble = new Mock<IUserRepository>();
        userRepoDouble.Setup(ur => ur.Exists(userId)).ReturnsAsync(false);

        var collabRepoDouble = new Mock<ICollaboratorRepository>();

        var factory = new CollaboratorFactory(collabRepoDouble.Object, userRepoDouble.Object);


        // assert
        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(
            async () =>
                // act
                await factory.Create(userId, period)
        );

        Assert.Equal("User does not exist for the provided UserId.", exception.Message);
    }

    [Fact]
    public async Task CollaboratorFactory_CreateWithSameUserId_ShouldNotCreateNewCollaborator()
    {
        // arrange
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(initDate: DateTime.Today.AddDays(1), finalDate: DateTime.Today.AddDays(5));

        var userRepoDouble = new Mock<IUserRepository>();
        userRepoDouble.Setup(ur => ur.Exists(userId)).ReturnsAsync(true);

        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        collabRepoDouble.Setup(cr => cr.ExistsByUserIdAsync(userId)).ReturnsAsync(true);

        var factory = new CollaboratorFactory(collabRepoDouble.Object, userRepoDouble.Object);

        // assert
        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(
            async () =>
                // act
                await factory.Create(userId, period)
        );

        Assert.Equal("A collaborator with this UserId already exists.", exception.Message);
    }

    [Fact]
    public void CollaboratorFactory_WhenCreatingCollaboratorWithICollaboratorVisitor_ShouldCreateNewCollaborator()
    {
        // Arrange
        var visitor = new Mock<ICollaboratorVisitor>();
        visitor.Setup(v => v.Id).Returns(It.IsAny<Guid>());
        visitor.Setup(v => v.UserId).Returns(It.IsAny<Guid>());
        visitor.Setup(v => v.PeriodDateTime).Returns(It.IsAny<PeriodDateTime>());

        var collabRepo = new Mock<ICollaboratorRepository>();
        var userRepo = new Mock<IUserRepository>();
        var collabFactory = new CollaboratorFactory(collabRepo.Object, userRepo.Object);

        // Act
        var result = collabFactory.Create(visitor.Object);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void CollaboratorFactory_WhenCreatingWithValidData_ShouldCreateNewCollaborator()
    {
        // arrange
        var collabId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(initDate: DateTime.Today.AddDays(1), finalDate: DateTime.Today.AddDays(5));

        var userRepoDouble = new Mock<IUserRepository>();
        userRepoDouble.Setup(ur => ur.Exists(userId)).ReturnsAsync(false);

        var collabRepoDouble = new Mock<ICollaboratorRepository>();

        var factory = new CollaboratorFactory(collabRepoDouble.Object, userRepoDouble.Object);

        // act
        var result = factory.Create(collabId, userId, period);

        // assert
        Assert.Equal(collabId, result.Id);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(period, result.PeriodDateTime);
    }
}
