using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.DTO.Collaborators
{
    public class CreateCollabDTO
    {
        public Guid? UserId { get; set; }
        public PeriodDateTime PeriodDateTime { get; set; } = null!;

        public string? Names { get; set; }
        public string? Surnames { get; set; }
        public string? Email { get; set; }
        public DateTime? FinalDate { get; set; }
    } 
}