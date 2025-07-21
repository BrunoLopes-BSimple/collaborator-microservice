using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.IPublishers;
using Domain.Models;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using InterfaceAdapters.Consumers;
using Xunit;
using Domain.Messages;

namespace InterfaceAdapters.IntegrationTests.MessagingIntegrationTests
{
    public class MessagingIntegrationTests
    {
        [Fact]
        public async Task When_CollaboratorCreatedEvent_IsPublished_ConsumerShould_ConsumeIt()
        {
            //arrange
            var collabServiceDouble = new Mock<ICollaboratorService>();

            await using var provider = new ServiceCollection()

                .AddSingleton<ICollaboratorService>(collabServiceDouble.Object)

                .AddMassTransitTestHarness(cfg =>
                {
                    cfg.AddConsumer<CollaboratorConsumer>();
                })
                .BuildServiceProvider(true);

            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();

            try
            {
                //Act
                var message = new CollaboratorCreatedMessage(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1))
                );

                await harness.Bus.Publish(message);

                // Assert

                Assert.True(await harness.GetConsumerHarness<CollaboratorConsumer>().Consumed.Any<CollaboratorCreatedMessage>());

                collabServiceDouble.Verify(
                    s => s.AddCollaboratorReferenceAsync(message.Id, message.UserId, message.PeriodDateTime),
                    Times.Once
                );
            }
            finally
            {
                await harness.Stop();
            }
        }

        [Fact]
        public async Task When_CollaboratorUpdatedEvent_IsPublished_ConsumerShould_ConsumeIt()
        {
            //Arrange
            var collabServiceDouble = new Mock<ICollaboratorService>();

            await using var provider = new ServiceCollection()
                .AddSingleton<ICollaboratorService>(collabServiceDouble.Object)
                .AddMassTransitTestHarness(cfg =>
                {
                    cfg.AddConsumer<CollaboratorUpdatedConsumer>();
                })
                .BuildServiceProvider(true);

            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();

            try
            {
                //Act
                var message = new CollaboratorUpdatedMessage(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1))
                );

                await harness.Bus.Publish(message);

                //Assert
                Assert.True(await harness.GetConsumerHarness<CollaboratorUpdatedConsumer>().Consumed.Any<CollaboratorUpdatedMessage>());

                collabServiceDouble.Verify(
                    s => s.UpdateCollaboratorReferenceAsync(message.Id, message.UserId, message.PeriodDateTime),
                    Times.Once
                );
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}