using Application.DTO.Collaborators;
using Application.Interfaces;
using Domain.Messages;
using MassTransit;

namespace WebApi.Saga;

public class CollaboratorCreatedStateMachine : MassTransitStateMachine<CollaboratorCreatedState>
{
    public State WaitingForUserCreation { get; private set; }
    public Event<CollaboratorWithoutUserStartSagaMessage> CollaboratorWithoutUserStartSagaMessage { get; private set; }
    public Event<UserFromCollaboratorCreatedMessage> UserFromCollaboratorCreatedMessage { get; private set; }

    public CollaboratorCreatedStateMachine()
    {

        InstanceState(x => x.CurrentState);

        Event(() => CollaboratorWithoutUserStartSagaMessage, x =>
        {
            x.CorrelateById(context => context.Message.CorrelationId);
            x.InsertOnInitial = true;
            x.SetSagaFactory(context => new CollaboratorCreatedState
            {
                CorrelationId = context.Message.CorrelationId,
                Names = context.Message.Names,
                Surnames = context.Message.Surnames,
                Email = context.Message.Email,
                CollaboratorPeriodDateTime = context.Message.PeriodDateTime
            });
        });

        Event(() => UserFromCollaboratorCreatedMessage, x =>
            x.CorrelateBy((saga, context) => saga.Email == context.Message.Email)
        );

        Initially(
            When(CollaboratorWithoutUserStartSagaMessage)
                .ThenAsync(async context =>
                {
                    Console.WriteLine("[DEBUG] MACHINE --- FIRST STAGE ---  CollaboratorWithoutUserStartSagaMessage ---" + InstanceInfo.InstanceId);
                    var msg = context.Message;

                    var serviceProvider = context.GetPayload<IServiceProvider>();
                    var collaboratorService = serviceProvider.GetService<ICollaboratorService>();
                    var createCollabDto = new CreateCollaboratorWithoutUserDTO(msg.Names, msg.Surnames, msg.Email, msg.DeactivationDate, msg.PeriodDateTime);

                    await collaboratorService.CreateWithoutUser(createCollabDto, msg.CorrelationId);
                })
                .TransitionTo(WaitingForUserCreation)
        );

        During(WaitingForUserCreation,
            When(UserFromCollaboratorCreatedMessage)
                .ThenAsync(async context =>
                {
                    Console.WriteLine("[DEBUG] MACHINE --- FIRST STAGE ---  UserFromCollaboratorCreatedMessage ---" + InstanceInfo.InstanceId);

                    var saga = context.Saga;

                    saga.UserId = context.Message.UserId;
                    saga.UserPeriodDateTime = context.Message.PeriodDateTime;

                    var serviceProvider = context.GetPayload<IServiceProvider>();
                    var collaboratorService = serviceProvider.GetService<ICollaboratorService>();

                    await collaboratorService.AddUserIdForCollaboratorAsync(context.Message.UserId, context.Message.CollaboratorId);
                })
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }
}
