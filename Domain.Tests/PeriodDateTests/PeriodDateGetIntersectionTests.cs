using Domain.Models;

namespace Domain.Tests.PeriodDateTests;

public class PeriodDateGetIntersectionTests
{
    public static IEnumerable<object[]> PeriodsThatIntersect()
    {
        yield return new object[] {
                new PeriodDate(new DateOnly(2021, 1, 1), new DateOnly(2022, 1, 1)),
                new PeriodDate(new DateOnly(2021, 1, 1), new DateOnly(2021, 1, 1))
            };
        yield return new object[] {
                new PeriodDate(new DateOnly(2019, 1, 1), new DateOnly(2020, 1, 1)),
                new PeriodDate(new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 1))
            };
    }


    [Theory]
    [MemberData(nameof(PeriodsThatIntersect))]
    public void WhenPassingPeriod_ThenReturnIntersection(PeriodDate periodDate2, PeriodDate intersectionPeriod)
    {
        //arrange
        DateOnly initDate = new DateOnly(2020, 1, 1);
        DateOnly finalDate = new DateOnly(2021, 1, 1);

        PeriodDate periodDate = new PeriodDate(initDate, finalDate);

        //act
        var result = periodDate.GetIntersection(periodDate2);

        //assert
        Assert.NotNull(result);
        Assert.Equal(intersectionPeriod.GetInitDate(), result.GetInitDate());
        Assert.Equal(intersectionPeriod.GetFinalDate(), result.GetFinalDate());
    }

    [Fact]
    public void WhenPeriodsDontIntersect_ThenReturnNull()
    {
        //arrange
        DateOnly initDate = new DateOnly(2020, 1, 1);
        DateOnly finalDate = new DateOnly(2021, 1, 1);

        PeriodDate periodDate = new PeriodDate(initDate, finalDate);

        DateOnly initDate2 = new DateOnly(2018, 1, 1);
        DateOnly finalDate2 = new DateOnly(2019, 1, 1);

        PeriodDate periodDate2 = new PeriodDate(initDate2, finalDate2);

        //act
        var result = periodDate.GetIntersection(periodDate2);

        //assert
        Assert.Null(result);
    }

    [Fact]
    public void GetIntersection_WithNonOverlappingPeriods_ShouldReturnNull()
    {
        // Arrange
        var period1 = new PeriodDate(new DateOnly(2024, 7, 1), new DateOnly(2024, 7, 5));
        var period2 = new PeriodDate(new DateOnly(2024, 7, 6), new DateOnly(2024, 7, 10));

        // Act
        var intersection = period1.GetIntersection(period2);

        // Assert
        Assert.Null(intersection);
    }

}
