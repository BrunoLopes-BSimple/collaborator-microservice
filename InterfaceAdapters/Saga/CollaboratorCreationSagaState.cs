using Domain.Models;
using MassTransit;

namespace InterfaceAdapters.Saga
{
    public class CollaboratorCreationSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string? CurrentState { get; set; }
        public Guid CollaboratorId { get; set; }
        public string? Email { get; set; }
        public PeriodDateTime CollaboratorPeriod{ get; set; }
    }
}