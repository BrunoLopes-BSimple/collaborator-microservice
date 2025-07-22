using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.CollaboratorWithoutUserRepositoryTests;

public class CollabWUDeleteAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenIdExists_ShouldDeleteTempCollaborator()
    {
        // Arrange
        var collabId = Guid.NewGuid();
        var names = "name";
        var surname = "surname";
        var email = "email@gmail.com";
        var deactivationDate = DateTime.Now.AddDays(100);
        var period = new PeriodDateTime(DateTime.Now.AddDays(5), DateTime.Now.AddDays(50));

        var collaboratorDM1 = new CollaboratorWithoutUserDataModel(collabId, names, surname, email, deactivationDate, period);
        context.TempCollaborators.Add(collaboratorDM1);
        await context.SaveChangesAsync();

        var expected = new Mock<ICollaboratorWithoutUser>().Object;

        _mapper.Setup(m => m.Map<CollaboratorWithoutUserDataModel, CollaboratorWithoutUser>(
        It.Is<CollaboratorWithoutUserDataModel>(t =>
            t.Id == collaboratorDM1.Id
            )))
            .Returns(new CollaboratorWithoutUser(collabId, names, surname, email, deactivationDate, period));

        var collaboratorRepository = new CollaboratorWithoutUserRepository(context, _mapper.Object);

        var insertedTempCollab = await collaboratorRepository.GetByIdAsync(collabId);

        Assert.NotNull(insertedTempCollab);
        Assert.Equal(collaboratorDM1.Id, insertedTempCollab.Id);

        // act
        await collaboratorRepository.DeleteAsync(collabId);

        // assert
        var result = await collaboratorRepository.GetByIdAsync(collabId);
        Assert.Null(result);
    }
}
