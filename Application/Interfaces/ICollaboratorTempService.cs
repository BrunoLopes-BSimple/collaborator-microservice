using Application.DTO.CollaboratorTemp;
using Domain.Models;

namespace Application.Interfaces;

public interface ICollaboratorTempService
{
    public Task<Result> StartCreate(CreateCollaboratorTempDTO createDTO);
    public Task<Result<CreatedCollaboratorTempDTO>> Create(PeriodDateTime periodDateTime, string names, string surnames, string email, DateTime finalDate);
    public Task<Result> Remove(Guid collabId);
}
