using Application.DTO.Collaborators;
using Domain.Models;

namespace WebApi.IntegrationTests.Helpers;

public static class CollaboratorHelper
{
    public static CreateCollabDTO GenerateCreateCollabDto(Guid userId)
    {
        return new CreateCollabDTO
        {
            UserId = userId,
            PeriodDateTime = new PeriodDateTime(DateTime.UtcNow, DateTime.UtcNow.AddYears(1))
        };
    }
}
