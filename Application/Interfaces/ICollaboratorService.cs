using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTO;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Interfaces
{
    public interface ICollaboratorService
    {
        Task<ICollaborator?> AddCollaboratorReferenceAsync(Guid collabId, Guid userId, PeriodDateTime period);

        Task<Result<CreatedCollaboratorDTO>> Create(CreateCollaboratorDTO collabDto);
    }
}