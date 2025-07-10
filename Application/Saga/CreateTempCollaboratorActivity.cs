using Application.Interfaces;
using Application.ISenders;
using Domain.Messages;
using MassTransit;

namespace Application.Saga;

public class CreateTempCollaboratorActivity : ICreateTempCollaboratorActivity
{
    private readonly ICollaboratorTempService _collaboratorTempService;
    private readonly IMessageSender _messageSender;

    public CreateTempCollaboratorActivity(ICollaboratorTempService collaboratorTempService, IMessageSender messageSender)
    {
        _collaboratorTempService = collaboratorTempService;
        _messageSender = messageSender;
    }

    public async Task Execute(BehaviorContext<CollaboratorCreatedState, CollaboratorTempCreationCommandMessage> context, IBehavior<CollaboratorCreatedState, CollaboratorTempCreationCommandMessage> next)
    {
        var msg = context.Message;

        var collabTemp = await _collaboratorTempService.Create(msg.PeriodDateTime, msg.Names, msg.Surnames, msg.Email, msg.FinalDate);

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
    }

    public Task Faulted<TException>(BehaviorExceptionContext<CollaboratorCreatedState, CollaboratorTempCreationCommandMessage, TException> context, IBehavior<CollaboratorCreatedState, CollaboratorTempCreationCommandMessage> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context) => context.CreateScope("create-temp-collaborator");
    public void Accept(StateMachineVisitor visitor) => visitor.Visit(this);
}