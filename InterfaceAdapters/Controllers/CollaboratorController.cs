using Application.DTO;
using Application.DTO.Collaborators;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InterfaceAdapters.Controllers;

[Route("api/collaborators")]
[ApiController]
public class CollaboratorController : ControllerBase
{
    private readonly ICollaboratorService _collabService;

    public CollaboratorController(ICollaboratorService collabService)
    {
        _collabService = collabService;
    }

    [HttpPost]
    public async Task<ActionResult<CreatedCollaboratorDTO>> Create([FromBody] CreateCollabDTO collabDto)
    {
        if (collabDto.UserId.HasValue)
        {
            var createCollabDto = new CreateCollaboratorDTO((Guid)collabDto.UserId, collabDto.PeriodDateTime);
            var collabCreated = await _collabService.Create(createCollabDto);
            return collabCreated.ToActionResult();
        }

        if (collabDto.FinalDate < DateTime.UtcNow)
            return BadRequest("Missing required fields for temp collaborator creation.");

        var collabWithoutUserDTO = new CollabWithoutUserDTO(collabDto.Names, collabDto.Surnames, collabDto.Email, collabDto.FinalDate, collabDto.PeriodDateTime);
        var result = await _collabService.CreateCollaboratorWithoutUser(collabWithoutUserDTO);

        var returnResult = result.ToActionResult();
        return returnResult;
    }

    [HttpPut]
    public async Task<ActionResult<CollabUpdatedDTO>> updateCollaborator([FromBody] CollabDetailsDTO newCollabData)
    {
        var collabData = new CollabData(newCollabData.Id, newCollabData.PeriodDateTime);

        var result = await _collabService.EditCollaborator(collabData);
        return result.ToActionResult();
    }

}