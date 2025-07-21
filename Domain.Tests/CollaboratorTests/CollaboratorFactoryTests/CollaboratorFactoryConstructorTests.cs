using Domain.Factory;
using Domain.IRepository;
using Moq;

namespace Domain.Tests.CollaboratorTests.CollaboratorFactoryTests;

public class CollaboratorFactoryConstructorTests
{
    [Fact]
    public void CollaboratorFactory_Constructor_ShouldCreateNewFactory()
    {
        // arrange
        var repoDouble = new Mock<ICollaboratorRepository>();
        var userRepo = new Mock<IUserRepository>();

        // act
        new CollaboratorFactory(repoDouble.Object, userRepo.Object);
    }
}
