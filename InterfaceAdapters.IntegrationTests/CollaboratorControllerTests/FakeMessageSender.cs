using Application.ISenders;
using Domain.Models;

namespace InterfaceAdapters.IntegrationTests.CollaboratorControllerTests;

public class FakeMessageSender : IMessageSender
{
    public Task SendCollaboratorTempCreationCommandAsync(PeriodDateTime periodDateTime, string names, string surnames, string email, DateTime finalDate)
    {
        // Simulate success without doing anything
        return Task.CompletedTask;
    }

    public Task SendUserForCollabCommandAsync(Guid id, PeriodDateTime periodDateTime, string names, string surnames, string email, DateTime finalDate)
    {
        // Simulate success without doing anything
        return Task.CompletedTask;
    }
}
