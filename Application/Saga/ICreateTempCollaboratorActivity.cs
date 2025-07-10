using Domain.Messages;
using MassTransit;

namespace Application.Saga;

public interface ICreateTempCollaboratorActivity : IStateMachineActivity<CollaboratorCreatedState, CollaboratorTempCreationCommandMessage>
{
}
