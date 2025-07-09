using Domain.Models;

namespace Domain.Interfaces;
public interface ICollaboratorTemp
{
    public Guid Id { get; }
    public PeriodDateTime PeriodDateTime { get; }
    public string Names { get; }
    public string Surnames { get; }
    public string Email { get; }
    public DateTime FinalDate { get; }

}
