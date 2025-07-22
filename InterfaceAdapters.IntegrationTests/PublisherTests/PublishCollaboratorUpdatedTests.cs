using Domain.Interfaces;
using Domain.Messages;
using Domain.Models;
using InterfaceAdapters.Publishers;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.PublisherTests;

public class PublishCollaboratorUpdatedTests
{
    [Fact]
    public async Task PublishCollaboratorUpdatedAsync_ShouldPublishEventWithCorrectData()
    {
        // Arrange 
        var publishEndpointDouble = new Mock<IPublishEndpoint>();

        var publisher = new MassTransitPublisher(publishEndpointDouble.Object);

        var collaboratorDouble = new Mock<ICollaborator>();
        var collaboratorId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1));

        collaboratorDouble.Setup(c => c.Id).Returns(collaboratorId);
        collaboratorDouble.Setup(c => c.UserId).Returns(userId);
        collaboratorDouble.Setup(c => c.PeriodDateTime).Returns(period);

        // Act 
        await publisher.PublishCollaboratorUpdatedAsync(collaboratorDouble.Object);

        // Assert
        publishEndpointDouble.Verify(
            p => p.Publish(
                It.Is<CollaboratorUpdatedMessage>(e =>
                    e.Id == collaboratorId &&
                    e.UserId == userId &&
                    e.PeriodDateTime == period
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}
