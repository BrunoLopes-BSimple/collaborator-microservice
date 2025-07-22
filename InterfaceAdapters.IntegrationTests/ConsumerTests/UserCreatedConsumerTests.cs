using Application.Interfaces;
using Domain.Messages;
using Domain.Models;
using InterfaceAdapters.Consumers;
using MassTransit;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ConsumerTests;

public class UserCreatedConsumerTests
{
    [Fact]
    public async Task Consume_ShouldAddUserReferenceAsync_WithCorrectData()
    {
        // arrange
        var userServiceDouble = new Mock<IUserService>();
        var consumer = new UserCreatedConsumer(userServiceDouble.Object);

        var message = new UserCreatedMessage(Guid.NewGuid(), "name", "surname", "email@gmail.com", It.IsAny<PeriodDateTime>());

        var context = Mock.Of<ConsumeContext<UserCreatedMessage>>(c => c.Message == message);

        // act
        await consumer.Consume(context);

        // assert
        userServiceDouble.Verify(s => s.AddUserReferenceAsync(message.Id), Times.Once);
    }
}
