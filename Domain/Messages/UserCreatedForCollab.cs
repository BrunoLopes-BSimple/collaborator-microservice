using Domain.Models;

namespace Domain.Messages
{
    public record UserCreatedForCollab(Guid Id, string Names, string Surnames, string Email, PeriodDateTime PeriodDateTime);
}