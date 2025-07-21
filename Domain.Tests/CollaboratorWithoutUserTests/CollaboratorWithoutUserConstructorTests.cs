using Domain.Models;
using Moq;

namespace Domain.Tests.CollaboratorWithoutUserTests;

public class CollaboratorWithoutUserConstructorTests
{
    [Fact]
    public void CollaboratorWithoutUser_Constructor_ShoudCreateNewCollaboratorWithoutUser()
    {
        new CollaboratorWithoutUser(Guid.NewGuid(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<PeriodDateTime>());
    }
}
