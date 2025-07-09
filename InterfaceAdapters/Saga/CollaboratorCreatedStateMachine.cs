using Domain.Messages;
using MassTransit;
using Application.Saga;

public class CollaboratorCreatedStateMachine : MassTransitStateMachine<CollaboratorCreatedState>
{
    public State WaitingForUserCreation { get; private set; } = null!;
    public Event<CollaboratorTempCreationCommandMessage> CollaboratorTempCreated { get; private set; } = null!;
    public Event<UserCreatedMessage> UserCreated { get; private set; } = null!;

    public CollaboratorCreatedStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => CollaboratorTempCreated, x =>
        {
            x.CorrelateBy((context, saga) => saga.Message.Email == context.Email);
            x.SelectId(context => Guid.NewGuid()); 
            x.InsertOnInitial = true;
        });

        Event(() => UserCreated, x => x.CorrelateBy((context, saga) => saga.Message.Email == context.Email));

        Initially(
            When(CollaboratorTempCreated)
                .Activity(x => x.OfType<CreateTempCollaboratorActivity>()) 
                .TransitionTo(WaitingForUserCreation)
        );

        During(WaitingForUserCreation,
            When(UserCreated)
                .Activity(x => x.OfType<ConvertIntoCollabActivity>())
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }
}
