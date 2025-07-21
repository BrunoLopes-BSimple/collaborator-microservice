using Domain.Factory.CollabWithoutUserFactory;
using Domain.Models;
using Domain.Visitor;
using Moq;

namespace Domain.Tests.CollaboratorWithoutUserTests.CollaboratorWithoutUserFactoryTests;

public class CollaboratorWithoutUserFactoryCreateTests
{
    [Fact]
    public void Create_WhenCreatinWithVisitor_ShouldCreateCollaboratorWithoutUser()
    {
        // arrange
        var collabId = Guid.NewGuid();
        var names = "John";
        var surnames = "Doe";
        var email = "john.doe@example.com";
        DateTime? deactivationDate = DateTime.Today.AddDays(30);
        var period = new PeriodDateTime(DateTime.Today, DateTime.Today.AddDays(5));

        var collabVisitorDouble = new Mock<ICollaboratorWithoutUserVisitor>();
        collabVisitorDouble.Setup(c => c.Id).Returns(collabId);
        collabVisitorDouble.Setup(c => c.Names).Returns(names);
        collabVisitorDouble.Setup(c => c.Surnames).Returns(surnames);
        collabVisitorDouble.Setup(c => c.Email).Returns(email);
        collabVisitorDouble.Setup(c => c.DeactivationDate).Returns(deactivationDate);
        collabVisitorDouble.Setup(c => c.PeriodDateTime).Returns(period);

        var factory = new CollaboratorWithoutUserFactory();

        // act
        var collaborator = factory.Create(collabVisitorDouble.Object);

        // assert
        Assert.Equal(collabId, collaborator.Id);
        Assert.Equal(names, collaborator.Names);
        Assert.Equal(surnames, collaborator.Surnames);
        Assert.Equal(email, collaborator.Email);
        Assert.Equal(deactivationDate, collaborator.DeactivationDate);
        Assert.Equal(period, collaborator.PeriodDateTime);
    }

}
