using Application.Saga;
using Domain.Messages;
using Domain.Models;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.StateMachineTests;
public class CollaboratorCreatedStateMachineTests
{
    [Fact]
    public async Task Should_Transition_Through_States_And_Finalize()
    {
        // Arrange
        var collabId = Guid.NewGuid();
        var periodDateTime = new PeriodDateTime(DateTime.UtcNow.AddMonths(1), DateTime.UtcNow.AddMonths(3));
        var names = "John";
        var surnames = "Doe";
        var email = "johnDoe@email.com";
        var finalDate = DateTime.UtcNow.AddYears(1);

        var createTempActivityMock = new Mock<ICreateTempCollaboratorActivity>();
        createTempActivityMock
            .Setup(x => x.Execute(It.IsAny<BehaviorContext<CollaboratorCreatedState, CollaboratorTempCreationCommandMessage>>(), It.IsAny<IBehavior<CollaboratorCreatedState, CollaboratorTempCreationCommandMessage>>()))
            .Returns<BehaviorContext<CollaboratorCreatedState, CollaboratorTempCreationCommandMessage>, IBehavior<CollaboratorCreatedState, CollaboratorTempCreationCommandMessage>>(async (context, next) =>
            {
                await next.Execute(context);
            });

        var convertIntoCollabActivityMock = new Mock<IConvertIntoCollabActivity>();
        convertIntoCollabActivityMock
            .Setup(x => x.Execute(It.IsAny<BehaviorContext<CollaboratorCreatedState, UserCreatedMessage>>(), It.IsAny<IBehavior<CollaboratorCreatedState, UserCreatedMessage>>()))
            .Returns<BehaviorContext<CollaboratorCreatedState, UserCreatedMessage>, IBehavior<CollaboratorCreatedState, UserCreatedMessage>>(async (context, next) =>
            {
                await next.Execute(context);
            });

        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddSingleton<ICreateTempCollaboratorActivity>(createTempActivityMock.Object);
                cfg.AddSingleton<IConvertIntoCollabActivity>(convertIntoCollabActivityMock.Object);
                cfg.AddSagaStateMachine<CollaboratorCreatedStateMachine, CollaboratorCreatedState>();

            })
            .BuildServiceProvider();

        var harness = provider.GetRequiredService<ITestHarness>();

        await harness.Start();
        // Act: Publish initial event to create saga
        await harness.Bus.Publish(new CollaboratorTempCreationCommandMessage
        (
            periodDateTime,
            names,
            surnames,
            email,
            finalDate
        ));

        // Assert event consumed by bus
        Assert.True(await harness.Consumed.Any<CollaboratorTempCreationCommandMessage>());

        var sagaHarness = harness.GetSagaStateMachineHarness<CollaboratorCreatedStateMachine, CollaboratorCreatedState>();

        // Assert saga consumed the event
        Assert.True(await sagaHarness.Consumed.Any<CollaboratorTempCreationCommandMessage>());

        // Wait for saga instance to be created
        Assert.True(await sagaHarness.Created.Any(x => x.Email == email));

        // Get saga instance related to the sent command message
        var createdInstance = sagaHarness.Created.Select(x => x.Email == email).FirstOrDefault();

        // Saga exists
        Assert.NotNull(createdInstance);
        Assert.Equal(email, createdInstance.Saga.Email);
        
        // Assert saga is in correct intermediate state
        Assert.Equal("WaitingForUserCreation", createdInstance.Saga.CurrentState);

        // Act: Publish second event to complete saga
        await harness.Bus.Publish(new UserCreatedMessage(
            Guid.NewGuid(), names, surnames, email, periodDateTime));

        // Assert that event is caught by saga
        Assert.True(await harness.Consumed.Any<UserCreatedMessage>(), "UserCreatedMessage not consumed by bus");
        Assert.True(await sagaHarness.Consumed.Any<UserCreatedMessage>(), "UserCreatedMessage not consumed by saga");

    }

}