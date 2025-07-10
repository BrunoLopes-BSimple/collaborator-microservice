using InterfaceAdapters.Activities;
using Domain.Contracts;
using MassTransit;
using Domain.Messages;

namespace InterfaceAdapters.Saga
{
    public class CollaboratorCreationSagaStateMachine : MassTransitStateMachine<CollaboratorCreationSagaState>
    {
        public State PendingUserCreation { get; private set; } = null!;

        public Event<CreateCollaboratorCommand> CollaboratorTempCreated { get; private set; } = null!;
        public Event<UserCreatedForCollab> UserCreated { get; private set; } = null!;

        public CollaboratorCreationSagaStateMachine()
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
                .Activity(x => x.OfType<CreateTempCollabActivity>())
                .TransitionTo(PendingUserCreation)
            );

            During(PendingUserCreation,
                When(UserCreated)
                    .Activity(x => x.OfType<FinalizeCollaboratorActivity>())
                    .Finalize()
            );

            SetCompletedWhenFinalized();
        }
    }
}