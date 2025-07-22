using Domain.Contracts;
using Domain.Models;
using InterfaceAdapters.Sender;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.SenderTests;

public class SendUserForCollabCommandTests
{
    [Fact]
    public async Task SendUserForCollabCommandAsync_ShouldSendMessageToCorrectQueue()
    {
        // Arrange
        var sendEndpointMock = new Mock<ISendEndpoint>();
        var sendEndpointProviderMock = new Mock<ISendEndpointProvider>();

        var expectedUri = new Uri("queue:user-for-collab");

        sendEndpointProviderMock.Setup(p => p.GetSendEndpoint(expectedUri)).ReturnsAsync(sendEndpointMock.Object);

        var sender = new MassTransitSender(sendEndpointProviderMock.Object);

        var command = new UserForCollabCommandMessage(Guid.NewGuid(), It.IsAny<PeriodDateTime>(), "name", "surname", "email", It.IsAny<DateTime>());

        // Act
        await sender.SendUserForCollabCommandAsync(command);

        // Assert
        sendEndpointProviderMock.Verify(p => p.GetSendEndpoint(expectedUri), Times.Once);
    }
}
