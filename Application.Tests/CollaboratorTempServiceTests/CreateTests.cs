using Application.Services;
using Domain.Factory.CollabWithoutUserFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.Visitor;
using Moq;

namespace Application.Tests.CollaboratorTempServiceTests;

public class CreateTests
{
    [Fact]
    public async Task Create_WhenPassingValidData_ShouldReturnSuccess()
    {
        // arrange
        var collabId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1));
        var names = "Some";
        var surnames = "Random";
        var email = "name@example.com";
        var finalDate = DateTime.Now.AddYears(5);

        var expected = new CollaboratorWithoutUser(collabId, names, surnames, email, finalDate, period);

        var collabWtUFactoryDouble = new Mock<ICollaboratorWithoutUserFactory>();
        collabWtUFactoryDouble.Setup(cf => cf.Create(It.IsAny<ICollaboratorWithoutUserVisitor>())).Returns(expected);

        var collabWtUserRepoDouble = new Mock<ICollaboratorWithoutUserRepository>();
        collabWtUserRepoDouble.Setup(r => r.AddAsync(It.IsAny<ICollaboratorWithoutUser>())).ReturnsAsync(expected);

        var service = new CollaboratorTempService(collabWtUserRepoDouble.Object, collabWtUFactoryDouble.Object);

        // act
        var result = await service.Create(period, names, surnames, email, finalDate);

        // assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(collabId, result.Value.Id);
        Assert.Equal(names, result.Value.Names);
        Assert.Equal(surnames, result.Value.Surnames);
        Assert.Equal(email, result.Value.Email);
        Assert.Equal(finalDate, result.Value.FinalDate);
        Assert.Equal(period, result.Value.PeriodDateTime);

        collabWtUFactoryDouble.Verify(f => f.Create(It.IsAny<ICollaboratorWithoutUserVisitor>()), Times.Once);
        collabWtUserRepoDouble.Verify(r => r.AddAsync(It.IsAny<ICollaboratorWithoutUser>()), Times.Once);
    }

    [Fact]
    public async Task Create_WhenErrorOccurs_ShouldReturnInternalServerError()
    {
        // arrange
        var collabId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1));
        var names = "Some";
        var surnames = "Random";
        var email = "name@example.com";
        var finalDate = DateTime.Now.AddYears(5);

        var expected = new CollaboratorWithoutUser(collabId, names, surnames, email, finalDate, period);

        var collabWtUFactoryDouble = new Mock<ICollaboratorWithoutUserFactory>();
        collabWtUFactoryDouble.Setup(cf => cf.Create(It.IsAny<ICollaboratorWithoutUserVisitor>())).Returns(expected);

        var collabWtUserRepoDouble = new Mock<ICollaboratorWithoutUserRepository>();
        collabWtUserRepoDouble.Setup(r => r.AddAsync(It.IsAny<ICollaboratorWithoutUser>())).ThrowsAsync(new Exception("Error"));

        var service = new CollaboratorTempService(collabWtUserRepoDouble.Object, collabWtUFactoryDouble.Object);

        // act
        var result = await service.Create(period, names, surnames, email, finalDate);

        // assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Error", result.Error.Message);

        collabWtUFactoryDouble.Verify(f => f.Create(It.IsAny<ICollaboratorWithoutUserVisitor>()), Times.Once);
        collabWtUserRepoDouble.Verify(r => r.AddAsync(It.IsAny<ICollaboratorWithoutUser>()), Times.Once);
    }
}
