using Application.Interfaces;
using Application.Services;
using Domain.Commands;
using Domain.Messages;
using MassTransit;

namespace WebApi.Saga;

public class CollaboratorCreatedStateMachine : MassTransitStateMachine<CollaboratorCreatedState>
{
    public State WaitingForUserCreation { get; private set; }
    public Event<CollaboratorWithoutUserCreatedMessage> CollaboratorWithoutUserCreatedMessage { get; private set; }
    public Event<UserFromCollaboratorCreatedMessage> UserFromCollaboratorCreatedMessage { get; private set; }

    public CollaboratorCreatedStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => CollaboratorWithoutUserCreatedMessage, x =>
        {
            x.CorrelateById(context => context.Message.Id);

            x.InsertOnInitial = true;
            x.SetSagaFactory(context => new CollaboratorCreatedState
            {
                CorrelationId = context.Message.Id,
                CollaboratorId = context.Message.Id,
                Names = context.Message.Names,
                Surnames = context.Message.Surnames,
                Email = context.Message.Email,
            });
        });

        Event(() => UserFromCollaboratorCreatedMessage, x =>
            x.CorrelateBy((saga, context) => saga.CollaboratorId == context.Message.CollaboratorId)
        );

        Initially(
            When(CollaboratorWithoutUserCreatedMessage)
                .ThenAsync(async context =>
                {
                    Console.WriteLine("[DEBUG] MACHINE GOOOOAT" + InstanceInfo.InstanceId);
                    var msg = context.Message;

                    var sendEndpoint = await context.GetSendEndpoint(new Uri("queue:users-cmd-saga"));
                    await sendEndpoint.Send(new CreateUserFromCollaboratorCommand(
                        InstanceInfo.InstanceId,
                        msg.Id,
                        msg.Names,
                        msg.Surnames,
                        msg.Email,
                        msg.DeactivationDate
                    )); ;
                })
                .TransitionTo(WaitingForUserCreation)
        );

        During(WaitingForUserCreation,
            When(UserFromCollaboratorCreatedMessage)
                .ThenAsync(async context =>
                {
                    var saga = context.Saga;

                    saga.UserId = context.Message.Id;
                    saga.UserPeriodDateTime = context.Message.PeriodDateTime;

                    var serviceProvider = context.GetPayload<IServiceProvider>();
                    var collaboratorService = serviceProvider.GetService<ICollaboratorService>();
                    var userService = serviceProvider.GetService<UserService>();

                    await collaboratorService.AddUserIdForCollaboratorAsync(context.Message.Id, context.Message.CollaboratorId);
                    await userService.AddUserReferenceAsync(context.Message.Id);

                    await context.Publish(new CollaboratorCreatedMessage(
                        saga.CollaboratorId,
                        saga.UserId,
                        context.Message.PeriodDateTime
                    ));
                })
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }
}
