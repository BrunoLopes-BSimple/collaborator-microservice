using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.CollaboratorRepositoryTests;

public class CollaboratorRepositoryAlreadyExistsAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task AlreadyExistsAsync_WhenCollaboratorExists_ReturnsTrue()
    {
        // Arrange
        var collabDouble = new Mock<ICollaborator>();

        var id = Guid.NewGuid();
        
        collabDouble.Setup(c => c.Id).Returns(id);
        collabDouble.Setup(c => c.UserId).Returns(Guid.NewGuid());

        var dataModel = new CollaboratorDataModel(collabDouble.Object);

        context.Collaborators.Add(dataModel);
        await context.SaveChangesAsync();

        var repo = new CollaboratorRepositoryEF(context, _mapper.Object);

        // Act
        var exists = await repo.AlreadyExistsAsync(id);

        // Assert
        Assert.True(exists);
    }

}
