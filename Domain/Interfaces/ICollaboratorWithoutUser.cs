using Domain.Models;

namespace Domain.Interfaces;

public interface ICollaboratorWithoutUser
{
    public Guid Id { get; }
    public string Names { get; }
    public string Surnames { get; }
    public string Email { get; }
    public DateTime DeactivationDate { get; }
    public PeriodDateTime PeriodDateTime { get; }
}
