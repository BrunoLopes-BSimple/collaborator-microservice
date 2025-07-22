using Domain.Contracts;
using Domain.Models;
using InterfaceAdapters.Sender;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.SenderTests;

public class SendCollaboratorCreationCommandTests
{
    [Fact]
    public async Task SendCollaboratorCreationCommandAsync_ShouldSendMessageToCorrectQueue()
    {
        // Arrange
        var sendEndpointMock = new Mock<ISendEndpoint>();
        var sendEndpointProviderMock = new Mock<ISendEndpointProvider>();

        var expectedUri = new Uri("queue:collaborator-creation-saga");

        sendEndpointProviderMock.Setup(p => p.GetSendEndpoint(expectedUri)).ReturnsAsync(sendEndpointMock.Object);

        var sender = new MassTransitSender(sendEndpointProviderMock.Object);

        var command = new CreateCollaboratorCommand(Guid.NewGuid(), "name", "surname", "email", It.IsAny<PeriodDateTime>(), It.IsAny<DateTime>());

        // Act
        await sender.SendCollaboratorCreationCommandAsync(command);

        // Assert
        sendEndpointProviderMock.Verify(p => p.GetSendEndpoint(expectedUri), Times.Once);
    }
}
