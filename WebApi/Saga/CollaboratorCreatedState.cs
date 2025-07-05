using Domain.Models;
using MassTransit;

namespace WebApi.Saga;

public class CollaboratorCreatedState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid CollaboratorId { get; set; }
    public PeriodDateTime CollaboratorPeriodDateTime { get; set; }
    public string Names { get; set; }
    public string Surnames { get; set; }
    public string Email { get; set; }
    public Guid UserId { get; set; }
    public PeriodDateTime UserPeriodDateTime { get; set; }
}
