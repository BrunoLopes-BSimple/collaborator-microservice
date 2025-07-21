using Domain.Contracts;
using Domain.Models;
using InterfaceAdapters.Activities;
using InterfaceAdapters.Saga;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.MessagingIntegrationTests
{
    public class MachineStateTests
    {
        [Fact]
        public async Task ShouldCreateCollaboratorSuccessfully_WhenEventsOccurInOrder()
        {
            await using var provider = new ServiceCollection().AddMassTransitTestHarness(cfg =>
            {
                cfg.AddSagaStateMachine<CollaboratorCreationSagaStateMachine, CollaboratorCreationSagaState>()
                   .InMemoryRepository();

                cfg.AddSingleton<CreateTempCollabActivity>();
                cfg.AddSingleton<FinalizeCollaboratorActivity>();
            })
            .BuildServiceProvider(true);

            var harness = provider.GetRequiredService<ITestHarness>();
            var machine = provider.GetRequiredService<CollaboratorCreationSagaStateMachine>();

            await harness.Start();

            try
            {
                // user details
                var userEmail = "user@email.com";
                var name = "user";
                var surname = "surname";
                var deactivationDate = DateTime.Today.AddYears(30);

                var correlationId = Guid.NewGuid();

                // collab details
                var collabId = Guid.NewGuid();
                var collabPeriod = new PeriodDateTime(DateTime.Today.AddYears(1), DateTime.Today.AddYears(20));

                var initialCommand = new CreateCollaboratorCommand(collabId, name, surname, userEmail, collabPeriod, deactivationDate);
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}