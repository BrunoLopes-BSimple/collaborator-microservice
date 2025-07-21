using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.CollaboratorRepositoryTests;

public class CollaboratorRepositoryGetActiveCollaboratorsTests : RepositoryTestBase
{
    [Fact]
    public async Task GetActiveCollaborators_WhenThereAreActiveCollaborators_ReturnsThem()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(10));

        var collabDouble = new Mock<ICollaborator>();
        collabDouble.Setup(c => c.Id).Returns(id);
        collabDouble.Setup(c => c.UserId).Returns(userId);
        collabDouble.Setup(c => c.PeriodDateTime).Returns(period);

        var collaboratorDM = new CollaboratorDataModel(collabDouble.Object);
        context.Collaborators.Add(collaboratorDM);
        await context.SaveChangesAsync();

        var expected = new Collaborator(collaboratorDM.Id, collaboratorDM.UserId, collaboratorDM.PeriodDateTime);
        _mapper.Setup(m => m.Map<CollaboratorDataModel, Collaborator>(It.IsAny<CollaboratorDataModel>()))
               .Returns(expected);

        var repo = new CollaboratorRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.GetActiveCollaborators();

        // Assert
        Assert.Single(result);
        Assert.Equal(expected.Id, result.First().Id);
    }


}
