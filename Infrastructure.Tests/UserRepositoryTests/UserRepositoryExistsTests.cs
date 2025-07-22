using Infrastructure.DataModel;
using Infrastructure.Repositories;

namespace Infrastructure.Tests.UserRepositoryTests;

public class UserRepositoryExistsTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenSearchingById_ThenReturnsTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var userDM = new UserDataModel(userId);
        context.ValidUserIds.Add(userDM);

        await context.SaveChangesAsync();

        var userRepo = new UserRepositoryEF(context, _mapper.Object);

        //Act 
        var result = await userRepo.Exists(userId);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task WhenUserDoesNotExist_ThenReturnsFalse()
    {
        // Arrange
        var userRepo = new UserRepositoryEF(context, _mapper.Object);

        //Act 
        var result = await userRepo.Exists(Guid.NewGuid());

        //Assert
        Assert.False(result);
    }
}
