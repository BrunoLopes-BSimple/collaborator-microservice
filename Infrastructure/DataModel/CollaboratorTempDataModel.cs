using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Infrastructure.DataModel;

public class CollaboratorTempDataModel : ICollaboratorTempVisitor
{
    public required Guid Id { get; set; }
    public required PeriodDateTime PeriodDateTime { get; set; }
    public required string Names {  get; set; }
    public required string Surnames {  get; set; }
    public required string Email {  get; set; }
    public required DateTime FinalDate { get; set; }

    public CollaboratorTempDataModel()
    {
    }
    
    public CollaboratorTempDataModel(ICollaboratorTemp collaboratorTemp)
    {
        Id = collaboratorTemp.Id;
        PeriodDateTime = collaboratorTemp.PeriodDateTime;
        Names = collaboratorTemp.Names;
        Surnames = collaboratorTemp.Surnames;
        Email = collaboratorTemp.Email;
        FinalDate = collaboratorTemp.FinalDate;
    }
}
