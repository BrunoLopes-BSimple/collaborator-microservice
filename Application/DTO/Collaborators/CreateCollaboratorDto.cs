using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.DTO
{
    public record CreateCollaboratorDTO
    {
        public Guid UserId { get; set; }
        public PeriodDateTime PeriodDateTime { get; set; }

        public CreateCollaboratorDTO(Guid userId, PeriodDateTime periodDateTime)
        {
            UserId = userId;
            PeriodDateTime = periodDateTime;
        }
    }
}