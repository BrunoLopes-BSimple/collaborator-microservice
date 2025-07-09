using Application.DTO;
using Application.DTO.Collaborators;
using Application.DTO.CollaboratorTemp;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InterfaceAdapters.Controllers;

[Route("api/collaborators")]
[ApiController]
public class CollaboratorController : ControllerBase
{
    private readonly ICollaboratorService _collabService;
    private readonly ICollaboratorTempService _collaboratorTempService;

    public CollaboratorController(ICollaboratorService collabService, ICollaboratorTempService collaboratorTempService)
    {
        _collabService = collabService;
        _collaboratorTempService = collaboratorTempService;
    }

    [HttpPost]
    public async Task<ActionResult<CreatedCollaboratorDTO>> CreateCollaborator([FromBody] CreateCollabDTO dto)
    {
        if (dto.UserId.HasValue)
        {
            // Normal creation
            var createDto = new CreateCollaboratorDTO((Guid)dto.UserId, dto.PeriodDateTime);
            var result = await _collabService.Create(createDto);

            return result.ToActionResult();
        }

        // Else: Create temp collaborator + user
        if (string.IsNullOrEmpty(dto.Names) || string.IsNullOrEmpty(dto.Surnames) || string.IsNullOrEmpty(dto.Email) || !dto.FinalDate.HasValue)
            return BadRequest("Missing required fields for temp collaborator creation.");

        var tempDto = new CreateCollaboratorTempDTO(dto.PeriodDateTime, dto.Names, dto.Surnames, dto.Email, dto.FinalDate.Value);
        var tempResult = await _collaboratorTempService.StartCreate(tempDto);

        if (tempResult.IsFailure)
            return BadRequest(tempResult.Error);

        return Accepted();
    }

    [HttpPut]
    public async Task<ActionResult<CollabUpdatedDTO>> UpdateCollaborator([FromBody] CollabDetailsDTO newCollabData)
    {
        var collabData = new CollabData(newCollabData.Id, newCollabData.PeriodDateTime);

        var result = await _collabService.EditCollaborator(collabData);
        if (result == null) return BadRequest("Invalid Arguments");
        return Ok(result);
    }
}