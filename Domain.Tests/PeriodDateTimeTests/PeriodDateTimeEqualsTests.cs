using Domain.Models;

namespace Domain.Tests.PeriodDateTimeTests;

public class PeriodDateTimeEqualsTests
{
    [Fact]
    public void Equals_WithSameInitAndFinalDates_ShouldReturnTrue()
    {
        // Arrange
        var init = DateTime.Today;
        var final = DateTime.Today.AddDays(5);
        var period1 = new PeriodDateTime(init, final);
        var period2 = new PeriodDateTime(init, final);

        // Act
        var result = period1.Equals(period2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WithDifferentInitDates_ShouldReturnFalse()
    {
        // Arrange
        var period1 = new PeriodDateTime(DateTime.Today, DateTime.Today.AddDays(5));
        var period2 = new PeriodDateTime(DateTime.Today.AddDays(1), DateTime.Today.AddDays(5));

        // Act
        var result = period1.Equals(period2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WithDifferentFinalDates_ShouldReturnFalse()
    {
        // Arrange
        var period1 = new PeriodDateTime(DateTime.Today, DateTime.Today.AddDays(5));
        var period2 = new PeriodDateTime(DateTime.Today, DateTime.Today.AddDays(10));

        // Act
        var result = period1.Equals(period2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WithNonPeriodDateTimeObject_ShouldReturnFalse()
    {
        // Arrange
        var period = new PeriodDateTime(DateTime.Today, DateTime.Today.AddDays(5));

        // Act
        var result = period.Equals("not a period");

        // Assert
        Assert.False(result);
    }
}
