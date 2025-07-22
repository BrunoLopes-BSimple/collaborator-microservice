using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.CollaboratorRepositoryTests;

public class CollaboratorRepositoryExistsByUderIdAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task ExistsByUserIdAsync_WhenUserExists_ReturnsTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var collabDouble = new Mock<ICollaborator>();
        collabDouble.Setup(c => c.UserId).Returns(userId);
        collabDouble.Setup(c => c.Id).Returns(Guid.NewGuid());

        var dataModel = new CollaboratorDataModel(collabDouble.Object);
        context.Collaborators.Add(dataModel);
        await context.SaveChangesAsync();

        var repo = new CollaboratorRepositoryEF(context, _mapper.Object);

        // Act
        var exists = await repo.ExistsByUserIdAsync(userId);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsByUserIdAsync_WhenUserDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var repo = new CollaboratorRepositoryEF(context, _mapper.Object);

        // Act
        var exists = await repo.ExistsByUserIdAsync(Guid.NewGuid());

        // Assert
        Assert.False(exists);
    }

}
