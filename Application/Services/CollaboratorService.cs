using Domain.IRepository;
using Domain.Interfaces;
using Domain.Factory;
using Domain.Models;
using Application.IPublishers;
using Application.Interfaces;
using Application.DTO;
using Application.DTO.Collaborators;
using Domain.Factory.CollabWithoutUserFactory;
using Infrastructure.Resolvers;
using Infrastructure.DataModel;
using Domain.Contracts;
using Application.ISender;
namespace Application.Services;

public class CollaboratorService : ICollaboratorService
{
    private ICollaboratorRepository _collaboratorRepository;
    private ICollaboratorFactory _collaboratorFactory;
    private readonly IMessagePublisher _publisher;
    private readonly IMessageSender _sender;

    public CollaboratorService(ICollaboratorRepository collaboratorRepository, ICollaboratorFactory collaboratorFactory, IMessagePublisher publisher, IMessageSender sender)
    {
        _collaboratorRepository = collaboratorRepository;
        _collaboratorFactory = collaboratorFactory;
        _publisher = publisher;
        _sender = sender;
    }

    public async Task<ICollaborator?> AddCollaboratorReferenceAsync(Guid collabId, Guid userId, PeriodDateTime period)
    {
        var collabAlreadyExists = await _collaboratorRepository.AlreadyExistsAsync(collabId);

        if (collabAlreadyExists) return null;

        var newCollab = _collaboratorFactory.Create(collabId, userId, period);

        return await _collaboratorRepository.AddAsync(newCollab);
    }

    public async Task<ICollaborator?> UpdateCollaboratorReferenceAsync(Guid collabId, Guid userId, PeriodDateTime period)
    {
        var existingCollab = await _collaboratorRepository.GetByIdAsync(collabId);

        if (existingCollab == null)
        {
            return null;
        }

        existingCollab.UpdatePeriod(period);

        return await _collaboratorRepository.UpdateCollaborator(existingCollab);
    }

    public async Task<Result<CreatedCollaboratorDTO>> Create(CreateCollaboratorDTO collabDto)
    {
        ICollaborator newCollab;
        try
        {
            newCollab = await _collaboratorFactory.Create(collabDto.UserId, collabDto.PeriodDateTime);
            newCollab = await _collaboratorRepository.AddAsync(newCollab);

            var result = new CreatedCollaboratorDTO(newCollab.UserId, newCollab.Id, newCollab.PeriodDateTime);

            await _publisher.PublishCollaboratorCreatedAsync(newCollab);
            return Result<CreatedCollaboratorDTO>.Success(result);
        }
        catch (ArgumentException ex)
        {
            return Result<CreatedCollaboratorDTO>.Failure(Error.InternalServerError(ex.Message));
        }
    }

    public async Task<Result<CollabUpdatedDTO>?> EditCollaborator(CollabData dto)
    {
        var collab = await _collaboratorRepository.GetByIdAsync(dto.Id);
        if (collab == null)
            return Result<CollabUpdatedDTO>.Failure(Error.NotFound("Collaborator not found."));


        collab.UpdatePeriod(dto.PeriodDateTime);

        var updateCollabDetails = await _collaboratorRepository.UpdateCollaborator(collab);

        if (updateCollabDetails == null)
            return Result<CollabUpdatedDTO>.Failure(Error.InternalServerError("Failed to update collaborator."));


        await _publisher.PublishCollaboratorUpdatedAsync(updateCollabDetails);
        var result = new CollabUpdatedDTO(dto.Id, dto.PeriodDateTime);
        return Result<CollabUpdatedDTO>.Success(result);
    }

    public async Task<Result> CreateCollaboratorWithoutUser(CollabWithoutUserDTO dto)
    {
        try
        {
            var startSagaMessage = new CreateCollaboratorCommand(
                Guid.NewGuid(),
                dto.Names,
                dto.Surnames,
                dto.Email,
                dto.CollaboratorPeriod,
                dto.UserDeactivationDate
            );

            await _sender.SendCollaboratorCreationCommandAsync(startSagaMessage);
            return Result.Success();
        }
        catch (ArgumentException ex)
        {
            return Result<ICollaboratorWithoutUser>.Failure(Error.BadRequest(ex.Message));
        }
    }

    public async Task FinalizeAsync(Guid collaboratorId, Guid userId, PeriodDateTime period)
    {
        var collabDM = new CollaboratorDataModel { Id = collaboratorId, UserId = userId, PeriodDateTime = period };
        var collab = _collaboratorFactory.Create(collabDM);

        ICollaborator createdCollab;
        try
        {
            createdCollab = await _collaboratorRepository.AddAsync(collab);
            await _publisher.PublishCollaboratorCreatedAsync(createdCollab);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred: {e.Message}");
        }

    }
}
