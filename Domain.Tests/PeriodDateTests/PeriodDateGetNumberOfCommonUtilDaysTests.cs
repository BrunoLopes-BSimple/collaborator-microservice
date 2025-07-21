using Domain.Models;

namespace Domain.Tests.PeriodDateTests;

public class PeriodDateGetNumberOfCommonUtilDaysTests
{
    [Fact]
    public void WhenPassingValidData_ThenRetunsNumberOfCommonUtilDays()
    {
        // arrange
        var expectedDays = 3;

        var initDate = new DateOnly(2025, 4, 14);
        var finalDate = new DateOnly(2025, 4, 16);

        var periodDate = new PeriodDate(initDate, finalDate);

        // act
        var result = periodDate.GetNumberOfCommonUtilDays();

        // assert
        Assert.Equal(result, expectedDays);
    }

    [Fact]
    public void WhenPassingValidDataWithWeekEnd_ThenRetunsNumberOfCommonUtilDays()
    {
        // arrange
        var expectedDays = 11;

        var initDate = new DateOnly(2025, 4, 1);
        var finalDate = new DateOnly(2025, 4, 15);

        var periodDate = new PeriodDate(initDate, finalDate);

        // act
        var result = periodDate.GetNumberOfCommonUtilDays();

        // assert
        Assert.Equal(result, expectedDays);
    }
}
