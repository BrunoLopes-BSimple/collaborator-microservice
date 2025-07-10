using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTO.Collaborators;
using Domain.Models;

namespace Application.Interfaces
{
    public interface ICollaboratorTempService
    {
        public Task<Result<CreatedCollaboratorTempDTO>> Create(PeriodDateTime periodDateTime, string names, string surnames, string email, DateTime finalDate);
    }
}