using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.IPublishers;
using Domain.Contracts;
using Domain.IRepository;
using Domain.Messages;
using InterfaceAdapters.Saga;
using MassTransit;

namespace InterfaceAdapters.Activities
{
    public class FinalizeCollaboratorActivity : IStateMachineActivity<CollaboratorCreationSagaState, UserCreatedForCollab>
    {
        private readonly ICollaboratorService _collaboratorService;
        private IMessagePublisher _messagePublisher;

        public FinalizeCollaboratorActivity(ICollaboratorService collaboratorService, IMessagePublisher messagePublisher)
        {
            _collaboratorService = collaboratorService;
            _messagePublisher = messagePublisher;
        }

        public async Task Execute(BehaviorContext<CollaboratorCreationSagaState, UserCreatedForCollab> context, IBehavior<CollaboratorCreationSagaState, UserCreatedForCollab> next)
        {
            await _collaboratorService.FinalizeAsync(context.Saga.CollaboratorId, context.Message.Id, context.Message.PeriodDateTime);
        }

        public void Accept(StateMachineVisitor visitor) => visitor.Visit(this);
        public void Probe(ProbeContext context) => context.CreateScope("finalize-collaborator");
        public Task Faulted<TException>(BehaviorExceptionContext<CollaboratorCreationSagaState, UserCreatedForCollab, TException> context, IBehavior<CollaboratorCreationSagaState, UserCreatedForCollab> next) where TException : Exception
        {
            return next.Faulted(context);
        }

    }
}