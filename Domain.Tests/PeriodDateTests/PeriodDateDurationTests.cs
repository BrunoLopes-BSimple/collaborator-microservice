using Domain.Models;

namespace Domain.Tests.PeriodDateTests;

public class PeriodDateDurationTests
{
    [Fact]
    public void Duration_WithSameInitAndFinalDate_ShouldReturnOne()
    {
        // Arrange
        var date = new DateOnly(2024, 7, 21);
        var period = new PeriodDate(date, date);

        // Act
        var result = period.Duration();

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void Duration_WithDifferentDates_ShouldReturnCorrectNumberOfDays()
    {
        // Arrange
        var start = new DateOnly(2024, 7, 1);
        var end = new DateOnly(2024, 7, 10);
        var period = new PeriodDate(start, end);

        // Act
        var result = period.Duration();

        // Assert
        Assert.Equal(10, result);
    }
}
