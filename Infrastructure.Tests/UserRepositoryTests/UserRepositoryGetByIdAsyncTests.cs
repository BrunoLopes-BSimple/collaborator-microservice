using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.UserRepositoryTests;

public class UserRepositoryGetByIdAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task WhenSearchingById_ThenReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
    

        var userDM = new UserDataModel(userId);
        context.ValidUserIds.Add(userDM);

        await context.SaveChangesAsync();

        var expected = new Mock<IUser>().Object;

        _mapper.Setup(m => m.Map<UserDataModel, User>(
        It.Is<UserDataModel>(t =>
            t.Id == userDM.Id
            )))
            .Returns(new User(userId));

        var userRepo = new UserRepositoryEF(context, _mapper.Object);

        //Act 
        var result = await userRepo.GetByIdAsync(userId);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(userDM.Id, result.Id);
    }

    [Fact]
    public async Task WhenSearchingByIdWithNoUsers_ThenReturnsNull()
    {
        // Arrange
        var userRepo = new UserRepositoryEF(context, _mapper.Object);

        //Act 
        var result = await userRepo.GetByIdAsync(Guid.NewGuid());

        //Assert
        Assert.Null(result);
    }
}
