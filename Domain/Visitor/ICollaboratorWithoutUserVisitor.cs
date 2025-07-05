
using Domain.Interfaces;
using Domain.Models;

namespace Domain.Visitor;

public interface ICollaboratorWithoutUserVisitor
{
    Guid Id { get; }
    public string Names { get; }
    public string Surnames { get; }
    public string Email { get; }
    public DateTime DeactivationDate { get; }
    public PeriodDateTime PeriodDateTime { get; }
}

