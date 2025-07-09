using Application.Interfaces;
using Application.IPublishers;
using Domain.Messages;
using MassTransit;

namespace Application.Saga
{
    public class ConvertIntoCollabActivity : IStateMachineActivity<CollaboratorCreatedState, UserCreatedMessage>
    {
        private ICollaboratorService _collaboratorService;
        private IMessagePublisher _messagePublisher;

        public ConvertIntoCollabActivity(ICollaboratorService collaboratorService, IMessagePublisher messagePublisher)
        {
            _collaboratorService = collaboratorService;
            _messagePublisher = messagePublisher;
        }

        public void Accept(StateMachineVisitor visitor) => visitor.Visit(this);

        public async Task Execute(BehaviorContext<CollaboratorCreatedState, UserCreatedMessage> context, IBehavior<CollaboratorCreatedState, UserCreatedMessage> next)
        {
            var collab = 
                await _collaboratorService.ConvertTempToFullCollaborator(context.Saga.CollaboratorId, context.Message.Id, context.Message.PeriodDateTime);

            if (collab.IsFailure)
                throw new Exception("Failed to convert collaborator");

            await _messagePublisher.PublishCollaboratorCreatedAsync(collab.Value);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<CollaboratorCreatedState, UserCreatedMessage, TException> context, IBehavior<CollaboratorCreatedState, UserCreatedMessage> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context) => context.CreateScope("convert-to-collaborator");
    }
}
