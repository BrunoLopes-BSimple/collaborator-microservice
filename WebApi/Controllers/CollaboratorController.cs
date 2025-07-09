using Application.DTO;
using Application.DTO.Collaborators;
using Application.DTO.CollaboratorTemp;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

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
    public async Task<ActionResult<CreatedCollaboratorDTO>> Create([FromBody] CreateCollabDTO collabDto)
    {
        var createCollabDto = new CreateCollaboratorDTO(collabDto.UserId, collabDto.PeriodDateTime);

        var collabCreated = await _collabService.Create(createCollabDto);

        return collabCreated.ToActionResult();
    }

    [HttpPut]
    public async Task<ActionResult<CollabUpdatedDTO>> UpdateCollaborator([FromBody] CollabDetailsDTO newCollabData)
    {
        var collabData = new CollabData(newCollabData.Id, newCollabData.PeriodDateTime);

        var result = await _collabService.EditCollaborator(collabData);
        if (result == null) return BadRequest("Invalid Arguments");
        return Ok(result);
    }

    [HttpPost("with-user")]
    public async Task<ActionResult<CreatedCollaboratorTempDTO>> CreateCollabAndUser([FromBody] CreateCollaboratorTempDTO createCollaboratorTempDTO)
    {
        var result = await _collaboratorTempService.StartCreate(createCollaboratorTempDTO);
        if (result.IsFailure) return BadRequest(result.Error);

        return Accepted();
    }
}