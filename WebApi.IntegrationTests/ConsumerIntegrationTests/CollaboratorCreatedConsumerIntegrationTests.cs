using Application.Interfaces;
using Domain.Messages;
using Domain.Models;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApi.Consumers;
using Xunit;

namespace WebApi.IntegrationTests.ConsumerIntegrationTests;

public class CollaboratorCreatedConsumerIntegrationTests
{
    [Fact]
    public async Task ShouldConsumeCollaboratorCreatedMessage()
    {
        var services = new ServiceCollection();

        var collaboratorServiceMock = new Mock<ICollaboratorService>();

        services.AddSingleton(collaboratorServiceMock.Object);

        services.AddMassTransitTestHarness(cfg =>
        {
            cfg.AddConsumer<CollaboratorCreatedConsumer>();
        });

        await using var provider = services.BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();

        await harness.Start();
        try
        {
            var message = new CollaboratorCreatedMessage(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new PeriodDateTime(DateTime.Now, DateTime.Now.AddDays(10))
            );

            await harness.Bus.Publish(message);

            Assert.True(await harness.Consumed.Any<CollaboratorCreatedMessage>());

            var consumerHarness = provider.GetRequiredService<IConsumerTestHarness<CollaboratorCreatedConsumer>>();
            Assert.True(await consumerHarness.Consumed.Any<CollaboratorCreatedMessage>());

            collaboratorServiceMock.Verify(x =>
                x.AddCollaboratorReferenceAsync(
                    message.Id, message.UserId, message.PeriodDateTime),
                Times.Once);
        }
        finally
        {
            await harness.Stop();
        }
    }
}
