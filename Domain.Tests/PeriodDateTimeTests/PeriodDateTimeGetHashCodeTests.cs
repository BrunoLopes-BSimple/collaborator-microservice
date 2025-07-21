using Domain.Models;

namespace Domain.Tests.PeriodDateTimeTests;

public class PeriodDateTimeGetHashCodeTests
{
    [Fact]
    public void GetHashCode_ForEqualObjects_ShouldReturnSameValue()
    {
        // Arrange
        var init = DateTime.Today;
        var final = DateTime.Today.AddDays(5);
        var period1 = new PeriodDateTime(init, final);
        var period2 = new PeriodDateTime(init, final);

        // Act
        var hash1 = period1.GetHashCode();
        var hash2 = period2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void GetHashCode_ForDifferentObjects_ShouldReturnDifferentValues()
    {
        // Arrange
        var period1 = new PeriodDateTime(DateTime.Today, DateTime.Today.AddDays(5));
        var period2 = new PeriodDateTime(DateTime.Today.AddDays(1), DateTime.Today.AddDays(6));

        // Act
        var hash1 = period1.GetHashCode();
        var hash2 = period2.GetHashCode();

        // Assert
        Assert.NotEqual(hash1, hash2);
    }
}
