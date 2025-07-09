using Application.DTO.Collaborators;
using Application.Interfaces;
using Domain.Messages;
using MassTransit;

namespace WebApi.Saga;

public class CollaboratorCreatedStateMachine : MassTransitStateMachine<CollaboratorCreatedState>
{
    public State WaitingForUserCreation { get; private set; }
    public Event<CollaboratorWithoutUserStartSagaMessage> CollaboratorWithoutUserStartSagaMessage { get; private set; }
    public Event<UserCreatedMessage> UserCreatedMessage { get; private set; }

    public CollaboratorCreatedStateMachine()
    {

        InstanceState(x => x.CurrentState);

        Event(() => CollaboratorWithoutUserStartSagaMessage, x =>
        {
            x.CorrelateBy((saga, context) => saga.Email == context.Message.Email);
            x.SelectId(context => Guid.NewGuid());
            x.SetSagaFactory(context => new CollaboratorCreatedState
            {
                Email = context.Message.Email,
            });
        });

        Event(() => UserCreatedMessage, x =>
        {
            x.CorrelateBy((saga, context) => saga.Email == context.Message.Email);
        }
        );

        Initially(
            When(CollaboratorWithoutUserStartSagaMessage)
                .ThenAsync(async context =>
                {
                    Console.WriteLine($"[DEBUG] MACHINE --- FIRST STAGE ---  Email: {context.Message.Email} ---");
                    var msg = context.Message;

                    var serviceProvider = context.GetPayload<IServiceProvider>();
                    var collaboratorService = serviceProvider.GetService<ICollaboratorService>();
                    var createCollabDto = new CreateCollaboratorWithoutUserDTO(msg.Names, msg.Surnames, msg.Email, msg.DeactivationDate, msg.PeriodDateTime);

                    await collaboratorService.CreateWithoutUser(createCollabDto);
                })
                .TransitionTo(WaitingForUserCreation)
        );

        During(WaitingForUserCreation,
            When(UserCreatedMessage)
                .ThenAsync(async context =>
                {
                    Console.WriteLine($"[DEBUG] MACHINE --- SECOND STAGE ---  Email: {context.Message.Email} ---");
                    var msg = context.Message;

                    var serviceProvider = context.GetPayload<IServiceProvider>();
                    var collaboratorService = serviceProvider.GetService<ICollaboratorService>();
                    var convertCollabTempDto = new ConvertCollaboratorTempDTO(msg.Id, msg.Email);

                    await collaboratorService.ConvertCollaboratorTempToCollaboratorAsync(convertCollabTempDto);
                })
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }
}
