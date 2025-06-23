using Application.Interfaces;
using Application.Messaging;
using Domain.Models;
using MassTransit;
using Moq;
using WebApi.Consumers;
using Xunit;

namespace WebApi.IntegrationTests.ConsumerTests
{
    public class CollaboratorConsumerTests
    {
        [Fact]
        public async Task Consume_ShouldCallAddCollaboratorReferenceAsync_WithCorrectData()
        {
            // arrange
            var serviceDouble = new Mock<ICollaboratorService>();
            var consumer = new CollaboratorConsumer(serviceDouble.Object);

            var message = new CollaboratorCreatedEvent(Guid.NewGuid(), Guid.NewGuid(), new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1)));

            var context = Mock.Of<ConsumeContext<CollaboratorCreatedEvent>>(c => c.Message == message);

            // act
            await consumer.Consume(context);

            // asset
            serviceDouble.Verify(s => s.AddCollaboratorReferenceAsync(message.Id, message.UserId, message.PeriodDateTime), Times.Once);
        }

    }
}