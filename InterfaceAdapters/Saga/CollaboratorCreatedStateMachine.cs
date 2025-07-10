using Domain.Messages;
using MassTransit;
using Application.Saga;

public class CollaboratorCreatedStateMachine : MassTransitStateMachine<CollaboratorCreatedState>
{
    public State WaitingForUserCreation { get; private set; } = null!;
    public Event<CollaboratorTempCreationCommandMessage> CollaboratorTempCreationCmd { get; private set; } = null!;
    public Event<UserCreatedMessage> UserCreated { get; private set; } = null!;

    public CollaboratorCreatedStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => CollaboratorTempCreationCmd, x =>
        {
            x.CorrelateBy((context, saga) => saga.Message.Email == context.Email);
            x.SelectId(context => Guid.NewGuid()); 
            x.InsertOnInitial = true;
        });

        Event(() => UserCreated, x => x.CorrelateBy((context, saga) => saga.Message.Email == context.Email));

        Initially(
            When(CollaboratorTempCreationCmd)
                .Then(x => x.Saga.Email = x.Message.Email)
                .Activity(x => x.OfType<ICreateTempCollaboratorActivity>())
                .TransitionTo(WaitingForUserCreation)
        );

        During(WaitingForUserCreation,
            When(UserCreated)
                .Activity(x => x.OfType<IConvertIntoCollabActivity>())
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }
}
