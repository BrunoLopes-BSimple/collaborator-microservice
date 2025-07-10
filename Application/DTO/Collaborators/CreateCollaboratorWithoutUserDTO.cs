using Domain.Models;

namespace Application.DTO.Collaborators
{
    public class CreateCollaboratorWithoutUserDTO
    {
        public string Names { get; set; } = null!;
        public string Surnames { get; set; } = null!;
        public string Email { get; set; }
        public DateTime UserDeactivationDate { get; set; }
        public PeriodDateTime CollaboratorPeriod { get; set; }

        public CreateCollaboratorWithoutUserDTO(string names, string surnames, string email, DateTime userDeactivationDate, PeriodDateTime collaboratorPeriod)
        {
            Names = names;
            Surnames = surnames;
            Email = email;
            UserDeactivationDate = userDeactivationDate;
            CollaboratorPeriod = collaboratorPeriod;
        }
    }
}