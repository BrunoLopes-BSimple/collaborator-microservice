using Domain.Messages;
using Domain.Models;
using InterfaceAdapters.Sender;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.SenderTests;

public class MassTransitSenderTests
{
    [Fact]
    public async Task SendCollaboratorTempCreationCommandAsync_Sends_Correct_Message()
    {
        // Arrange
        var period = new PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddDays(5));
        var names = "John";
        var surnames = "Doe";
        var email = "john.doe@example.com";
        var finalDate = DateTime.UtcNow.AddYears(1);

        var endpointMock = new Mock<ISendEndpoint>();
        var providerMock = new Mock<ISendEndpointProvider>();
        providerMock
            .Setup(p => p.GetSendEndpoint(new Uri("queue:collab-user-saga")))
            .ReturnsAsync(endpointMock.Object);

        var sender = new MassTransitSender(providerMock.Object);

        // Act
        await sender.SendCollaboratorTempCreationCommandAsync(period, names, surnames, email, finalDate);

        // Assert
        providerMock.Verify(p => p.GetSendEndpoint(new Uri("queue:collab-user-saga")), Times.Once);

        endpointMock.Verify(e => e.Send(
            It.Is<CollaboratorTempCreationCommandMessage>(m =>
                m.Email == email &&
                m.Names == names &&
                m.Surnames == surnames &&
                m.FinalDate == finalDate &&
                m.PeriodDateTime.Equals(period)), // assuming PeriodDateTime overrides Equals
            default), Times.Once);
    }

    [Fact]
    public async Task SendUserForCollabCommandAsync_Sends_Correct_Message()
    {
        // Arrange
        var period = new PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddDays(5));
        var names = "Jane";
        var surnames = "Smith";
        var email = "jane.smith@example.com";
        var finalDate = DateTime.UtcNow.AddYears(1);
        var id = Guid.NewGuid();

        var endpointMock = new Mock<ISendEndpoint>();
        var providerMock = new Mock<ISendEndpointProvider>();
        providerMock
            .Setup(p => p.GetSendEndpoint(new Uri("queue:user-for-collab")))
            .ReturnsAsync(endpointMock.Object);

        var sender = new MassTransitSender(providerMock.Object);

        // Act
        await sender.SendUserForCollabCommandAsync(id, period, names, surnames, email, finalDate);

        // Assert
        providerMock.Verify(p => p.GetSendEndpoint(new Uri("queue:user-for-collab")), Times.Once);

        endpointMock.Verify(e => e.Send(
            It.Is<UserForCollabCommandMessage>(m =>
                m.Id == id &&
                m.Email == email &&
                m.Names == names &&
                m.Surnames == surnames &&
                m.FinalDate == finalDate &&
                m.PeriodDateTime.Equals(period)),
            default), Times.Once);
    }
}
