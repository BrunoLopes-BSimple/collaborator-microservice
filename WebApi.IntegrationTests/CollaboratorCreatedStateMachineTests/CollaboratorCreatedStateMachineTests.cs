using System;
using System.Threading.Tasks;
using Application.DTO.Collaborators;
using Application.Interfaces;
using Domain.Commands;
using Domain.Messages;
using Domain.Models;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WebApi.Saga;
using Xunit;

namespace WebApi.IntegrationTests.CollaboratorCreatedStateMachineTests;

public class CollaboratorCreatedStateMachineTests
{
    /*     [Fact]
        public async Task Should_Execute_Saga_Workflow_Correctly()
        {
            var services = new ServiceCollection();

            var collabServiceMock = new Mock<ICollaboratorService>();
            services.AddSingleton(collabServiceMock.Object);

            services.AddMassTransitTestHarness(cfg =>
            {
                cfg.AddSagaStateMachine<CollaboratorCreatedStateMachine, CollaboratorCreatedState>()
                   .InMemoryRepository();
            });

            await using var provider = services.BuildServiceProvider(true);
            var harness = provider.GetRequiredService<ITestHarness>();

            await harness.Start();
            try
            {
                var names = "testName";
                var surnames = "testName";
                var email = "test@example.com";
                var deactivonDate = DateTime.UtcNow.AddMonths(1);
                var startMessage = new CollaboratorWithoutUserStartSagaMessage
                (
                    Guid.NewGuid(),
                    names,
                    surnames,
                    email,
                    deactivonDate,
                    new PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddDays(10))
                );

                await harness.Bus.Publish(startMessage);

                var sagaHarness = harness.GetSagaStateMachineHarness<CollaboratorCreatedStateMachine, CollaboratorCreatedState>();

                Assert.True(await sagaHarness.Consumed.Any<CollaboratorWithoutUserStartSagaMessage>());
                Assert.Equal(1, sagaHarness.Sagas.Count()); // há uma instância da saga

                var saga = sagaHarness.Created
                    .Select(x => x.Email == email)
                    .FirstOrDefault();
                Assert.NotNull(saga);

                var machine = provider.GetRequiredService<CollaboratorCreatedStateMachine>();

                Assert.True(await sagaHarness.Created.ContainsInState(saga.Saga.CorrelationId, machine.WaitingForUserCreation, machine.WaitingForUserCreation));

                collabServiceMock.Verify(x =>
                    x.CreateWithoutUser(It.Is<CreateCollaboratorWithoutUserDTO>(dto => dto.Email == email)),
                    Times.Once);

                var userPeriodDateTime = new PeriodDateTime(DateTime.Now, deactivonDate);
                var userCreated = new UserCreatedMessage(
                    Guid.NewGuid(),
                    names, surnames, email,
                    userPeriodDateTime);

                await harness.Bus.Publish(userCreated);

                Assert.True(await sagaHarness.Consumed.Any<UserCreatedMessage>());

                collabServiceMock.Verify(x =>
                    x.ConvertCollaboratorTempToCollaboratorAsync(
                        It.Is<ConvertCollaboratorTempDTO>(dto => dto.Email == email)),
                    Times.Once);
            }
            finally
            {
                await harness.Stop();
            }
        } */

    [Fact]
    public async Task ASampleTest()
    {
        var services = new ServiceCollection();

        var collabServiceMock = new Mock<ICollaboratorService>();
        services.AddSingleton(collabServiceMock.Object);

        services.AddMassTransitTestHarness(cfg =>
        {
            cfg.AddSagaStateMachine<CollaboratorCreatedStateMachine, CollaboratorCreatedState>()
               .InMemoryRepository()
               .Endpoint(e => e.Name = "collaborators-cmd-saga-Instance1"); ;
        });

        await using var provider = services.BuildServiceProvider(true);
        var harness = provider.GetRequiredService<ITestHarness>();

        await harness.Start();

        var names = "testName";
        var surnames = "testName";
        var email = "test@example.com";
        var deactivationDate = DateTime.UtcNow.AddMonths(1);
        var collabPeriodDateTime = new PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddDays(10));
        await using var scope = provider.CreateAsyncScope();
        var scopedProvider = scope.ServiceProvider;
        var sendEndpointProvider = scopedProvider.GetRequiredService<ISendEndpointProvider>();
        var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:collaborators-cmd-saga-Instance1"));
        await endpoint.Send(new CollaboratorWithoutUserStartSagaMessage
            (
                Guid.NewGuid(),
                names,
                surnames,
                email,
                deactivationDate,
                collabPeriodDateTime
            ));

        var sagaHarness = harness.GetSagaStateMachineHarness<CollaboratorCreatedStateMachine, CollaboratorCreatedState>();
        Assert.True(await sagaHarness.Consumed.Any<CollaboratorWithoutUserStartSagaMessage>());

        var instance = sagaHarness.Created.Select(x => x.Email == email).FirstOrDefault();
        Assert.NotNull(instance);

        Assert.Equal("WaitingForUserCreation", instance.Saga.CurrentState);

        var createCollabDto = new CreateCollaboratorWithoutUserDTO(names, names, email, deactivationDate, collabPeriodDateTime);
        collabServiceMock
            .Setup(p => p.CreateWithoutUser(It.IsAny<CreateCollaboratorWithoutUserDTO>()))
            .Returns(async () =>
            {
                var result = new CreatedCollaboratorWithoutUserDTO(
                    Guid.NewGuid(),
                    names,
                    surnames,
                    email,
                    deactivationDate,
                    collabPeriodDateTime
                );

                var sendEndpointProvider = provider.GetRequiredService<ISendEndpointProvider>();
                var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:users-cmd-saga"));
                await endpoint.Send(new CreateUserFromCollaboratorCommand(names, surnames, email, deactivationDate));

                return result;
            });

        var userPeriodDateTime = new PeriodDateTime(DateTime.Now, deactivationDate);
        await harness.Bus.Publish(new UserCreatedMessage(
            Guid.NewGuid(),
            names, surnames, email,
            userPeriodDateTime
        ));

        Assert.True(await sagaHarness.Consumed.Any<UserCreatedMessage>());

        Assert.Equal("Final", instance.Saga.CurrentState);
    }


}
