using Domain.Models;

namespace Domain.Messages;

public record UserForCollabCommandMessage(Guid Id, PeriodDateTime PeriodDateTime, string Names, string Surnames, string Email, DateTime FinalDate);
