using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.ISender;
using Domain.Contracts;
using Domain.Factory.CollabWithoutUserFactory;
using Domain.IRepository;
using Infrastructure.DataModel;
using InterfaceAdapters.Saga;
using MassTransit;
namespace InterfaceAdapters.Activities
{
    public class CreateTempCollabActivity : IStateMachineActivity<CollaboratorCreationSagaState, CreateCollaboratorCommand>
    {
        private readonly IMessageSender _messageSender;
        private readonly ICollaboratorTempService _collabTempService;

        public CreateTempCollabActivity(IMessageSender messageSender, ICollaboratorTempService collabTempService)
        {
            _messageSender = messageSender;
            _collabTempService = collabTempService;
        }

        public async Task Execute(BehaviorContext<CollaboratorCreationSagaState, CreateCollaboratorCommand> context, IBehavior<CollaboratorCreationSagaState, CreateCollaboratorCommand> next)
        {
            var msg = context.Message;

            context.Saga.Email = msg.Email;

            var collabTemp = await _collabTempService.Create(msg.PeriodDateTimeOfCollaborator, msg.Names, msg.Surnames, msg.Email, msg.DeactivationDateOfUser);

            if (collabTemp.IsFailure)
                throw new Exception("Failed to create temporary collaborator");

            context.Saga.CollaboratorId = collabTemp.Value.Id;

            await _messageSender.SendUserForCollabCommandAsync(
                collabTemp.Value.Id,
                collabTemp.Value.PeriodDateTime,
                collabTemp.Value.Names,
                collabTemp.Value.Surnames,
                collabTemp.Value.Email,
                collabTemp.Value.FinalDate
            );

            await next.Execute(context);
            Console.WriteLine("Passei ao proximo passo ----------------------------------------");
        }

        public void Accept(StateMachineVisitor visitor) => visitor.Visit(this);
        public void Probe(ProbeContext context) => context.CreateScope("create-temp-collab");
        public Task Faulted<TException>(BehaviorExceptionContext<CollaboratorCreationSagaState, CreateCollaboratorCommand, TException> context, IBehavior<CollaboratorCreationSagaState, CreateCollaboratorCommand> next) where TException : Exception => next.Faulted(context);
    }
}