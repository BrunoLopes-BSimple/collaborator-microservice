using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.IPublishers;
using MassTransit;
using Moq;
using WebApi.Consumers;
using Xunit;
using Domain.Messages;

namespace WebApi.IntegrationTests.ConsumerTests
{
    public class CollaboratorUpdatedConsumerTests
    {
        [Fact]
        public async Task Consume_ShouldCallUpdateCollaboratorReferenceAsync_WithCorrectData()
        {
            // Arrange
            var serviceDouble = new Mock<ICollaboratorService>();
            var consumer = new CollaboratorUpdatedConsumer(serviceDouble.Object);

            var message = new CollaboratorUpdatedMessage(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new Domain.Models.PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1))
            );

            var context = Mock.Of<ConsumeContext<CollaboratorUpdatedMessage>>(c => c.Message == message);

            // Act
            await consumer.Consume(context);

            // Assert
            serviceDouble.Verify(s => s.UpdateCollaboratorReferenceAsync(message.Id, message.UserId, message.PeriodDateTime), Times.Once);
        }
    }
}