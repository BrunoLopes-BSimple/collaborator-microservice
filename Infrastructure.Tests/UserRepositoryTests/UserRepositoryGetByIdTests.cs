using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.UserRepositoryTests;

public class UserRepositoryGetByIdTests : RepositoryTestBase
{
    [Fact]
    public void WhenSearchingById_ThenReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();


        var userDM = new UserDataModel(userId);
        context.ValidUserIds.Add(userDM);

        context.SaveChangesAsync();

        var expected = new Mock<IUser>().Object;

        _mapper.Setup(m => m.Map<UserDataModel, User>(
        It.Is<UserDataModel>(t =>
            t.Id == userDM.Id
            )))
            .Returns(new User(userId));

        var userRepo = new UserRepositoryEF(context, _mapper.Object);

        //Act 
        var result = userRepo.GetById(userId);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(userDM.Id, result.Id);
    }

    [Fact]
    public void WhenSearchingByIdWithNoUsers_ThenReturnsNull()
    {
        // Arrange
        var userRepo = new UserRepositoryEF(context, _mapper.Object);

        //Act 
        var result = userRepo.GetById(Guid.NewGuid());

        //Assert
        Assert.Null(result);
    }
}
