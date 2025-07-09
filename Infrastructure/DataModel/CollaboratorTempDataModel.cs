using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Infrastructure.DataModel;

public class CollaboratorTempDataModel : ICollaboratorTempVisitor
{
    public Guid Id { get; set; }
    public PeriodDateTime PeriodDateTime { get; set; }
    public string Names {  get; set; }
    public string Surnames {  get; set; }
    public string Email {  get; set; }
    public DateTime FinalDate { get; set; }

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
