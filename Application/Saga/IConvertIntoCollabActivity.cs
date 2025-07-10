using Domain.Messages;
using MassTransit;

namespace Application.Saga;

public interface IConvertIntoCollabActivity : IStateMachineActivity<CollaboratorCreatedState, UserCreatedMessage>
{
}
