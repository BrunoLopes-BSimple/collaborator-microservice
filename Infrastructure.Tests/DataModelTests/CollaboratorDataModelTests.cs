using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Moq;

namespace Infrastructure.Tests.DataModelTests;

public class CollaboratorDataModelTests
{
    [Fact]
    public void ShouldCreateDataModel_WhenPassingValidCollaborator()
    {
        // arrange
        var collabId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.Now.AddDays(1), DateTime.Now.AddDays(5));

        var collab = new Mock<ICollaborator>();
        collab.Setup(c => c.Id).Returns(collabId);
        collab.Setup(c => c.UserId).Returns(userId);
        collab.Setup(c => c.PeriodDateTime).Returns(period);

        // act
        new CollaboratorDataModel(collab.Object);
    }

    [Fact]
    public void ShouldCreateDataModel_WhenPassingValidData()
    {
        // arrange
        var collabId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.Now.AddDays(1), DateTime.Now.AddDays(5));

        // act
        new CollaboratorDataModel(collabId, userId, period);
    }
}
