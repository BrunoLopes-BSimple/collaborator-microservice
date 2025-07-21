using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.CollaboratorRepositoryTests;

public class CollaboratorRepositoryUpdateCollaboratorTests : RepositoryTestBase
{
    [Fact]
    public async Task UpdateCollaborator_WhenCollaboratorExists_UpdatesAndReturnsIt()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var initialPeriod = new PeriodDateTime(DateTime.Today, DateTime.Today.AddDays(5));
        var updatedPeriod = new PeriodDateTime(DateTime.Today, DateTime.Today.AddDays(10));

        var collabDouble = new Mock<ICollaborator>();
        collabDouble.Setup(c => c.Id).Returns(id);
        collabDouble.Setup(c => c.UserId).Returns(userId);
        collabDouble.Setup(c => c.PeriodDateTime).Returns(updatedPeriod);

        var collaboratorDM = new CollaboratorDataModel
        {
            Id = id,
            UserId = userId,
            PeriodDateTime = initialPeriod
        };

        context.Collaborators.Add(collaboratorDM);
        await context.SaveChangesAsync();

        var expected = new Collaborator(id, userId, updatedPeriod);
        _mapper.Setup(m => m.Map<CollaboratorDataModel, Collaborator>(It.Is<CollaboratorDataModel>(dm => dm.Id == id)))
               .Returns(expected);

        var repo = new CollaboratorRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.UpdateCollaborator(collabDouble.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Id, result.Id);
        Assert.Equal(expected.PeriodDateTime._finalDate, result.PeriodDateTime._finalDate);
    }

}
