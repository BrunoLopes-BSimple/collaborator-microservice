using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.DTO.Collaborators
{
    public class CollabWithoutUserDTO
    {
        public string Names { get; set; }
        public string Surnames { get; set; }
        public string Email { get; set; }
        public DateTime UserDeactivationDate { get; set; }
        public PeriodDateTime CollaboratorPeriod { get; set; }

        public CollabWithoutUserDTO(string names, string surnames, string email, DateTime userDeactivationDate, PeriodDateTime collaboratorPeriod)
        {
            Names = names;
            Surnames = surnames;
            Email = email;
            UserDeactivationDate = userDeactivationDate;
            CollaboratorPeriod = collaboratorPeriod;
        }
    }
}