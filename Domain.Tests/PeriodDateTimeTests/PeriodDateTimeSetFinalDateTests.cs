using Domain.Models;

namespace Domain.Tests.PeriodDateTimeTests;

public class PeriodDateTimeSetFinalDateTests
{
    [Fact]
    public void SetFinalDate_ShouldUpdateFinalDate()
    {
        // Arrange
        var init = DateTime.Today;
        var final = DateTime.Today.AddDays(5);
        var newFinal = DateTime.Today.AddDays(10);
        var period = new PeriodDateTime(init, final);

        // Act
        period.SetFinalDate(newFinal);

        // Assert
        Assert.Equal(newFinal, period._finalDate);
    }
}
