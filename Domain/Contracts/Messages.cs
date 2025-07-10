using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Contracts
{
    public record CreateCollaboratorCommand(
        Guid CollabId,
        string Names,
        string Surnames,
        string Email,
        PeriodDateTime PeriodDateTimeOfCollaborator,
        DateTime DeactivationDateOfUser
    );

    public record UserForCollabCommandMessage(
        Guid Id,
        PeriodDateTime PeriodDateTime,
        string Names,
        string Surnames,
        string Email,
        DateTime FinalDate
    );

    public record UserCreated(
        Guid CorrelationId,
        Guid UserId
    );
}