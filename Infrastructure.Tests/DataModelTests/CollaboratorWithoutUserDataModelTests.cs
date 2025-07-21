using Domain.Models;
using Infrastructure.DataModel;
using Moq;

namespace Infrastructure.Tests.DataModelTests;

public class CollaboratorWithoutUserDataModelTests
{
    [Fact]
    public void WhenPassingValidData_ShouldCreateDataModel()
    {
        new CollaboratorWithoutUserDataModel(Guid.NewGuid(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<PeriodDateTime>());
    }
}
