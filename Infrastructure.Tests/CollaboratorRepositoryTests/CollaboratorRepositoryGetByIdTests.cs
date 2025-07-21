using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.CollaboratorRepositoryTests;

public class CollaboratorRepositoryGetByIdTests : RepositoryTestBase
{
    [Fact]
    public void GetById_WhenCollaboratorExists_ReturnsCollaborator()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(10));

        var collabDouble = new Mock<ICollaborator>();
        collabDouble.Setup(c => c.Id).Returns(id);
        collabDouble.Setup(c => c.UserId).Returns(userId);
        collabDouble.Setup(c => c.PeriodDateTime).Returns(period);

        var dataModel = new CollaboratorDataModel(collabDouble.Object);
        context.Collaborators.Add(dataModel);
        context.SaveChanges();

        _mapper.Setup(m => m.Map<CollaboratorDataModel, Collaborator>(It.Is<CollaboratorDataModel>(d => d.Id == id)))
               .Returns(new Collaborator(id, userId, period));

        var repo = new CollaboratorRepositoryEF(context, _mapper.Object);

        // Act
        var result = repo.GetById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public void GetById_WhenCollaboratorDoesNotExist_ReturnsNull()
    {
        // Arrange
        var repo = new CollaboratorRepositoryEF(context, _mapper.Object);

        // Act
        var result = repo.GetById(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

}

