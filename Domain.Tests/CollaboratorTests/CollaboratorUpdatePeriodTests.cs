using Domain.Models;
using Moq;

namespace Domain.Tests.CollaboratorTests;

public class CollaboratorUpdatePeriodTests
{
    [Fact]
    public void Collaborator_UpdatePeriod_ShouldUpdatePeriod()
    {
        // arrange
        var expected = new PeriodDateTime(initDate: DateTime.Today.AddDays(1), finalDate: DateTime.Today.AddDays(5));
        var collab = new Collaborator(Guid.NewGuid(), Guid.NewGuid(), It.IsAny<PeriodDateTime>());

        // act
        collab.UpdatePeriod(expected);

        // assert
        Assert.Equal(expected, collab.PeriodDateTime);
    }
}
