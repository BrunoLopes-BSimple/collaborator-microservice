using Domain.Models;
using MassTransit;

namespace WebApi.Saga;

public class CollaboratorCreatedState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public string Email { get; set; }
}
