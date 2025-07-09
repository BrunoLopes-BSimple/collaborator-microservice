using Application.DTO.CollaboratorTemp;
using Application.Interfaces;
using Application.ISenders;
using Domain.Factory.CollaboratorTempFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
namespace Application.Services;

public class CollaboratorTempService : ICollaboratorTempService
{
    private readonly ICollaboratorTempRepository _tempRepository;
    private readonly ICollaboratorTempFactory _tempFactory;
    private readonly IMessageSender _messageSender;  
    public CollaboratorTempService(ICollaboratorTempRepository collaboratorTempRepository, ICollaboratorTempFactory collaboratorTempFactory, IMessageSender messageSender) 
    {
        _tempFactory = collaboratorTempFactory;
        _tempRepository = collaboratorTempRepository;
        _messageSender = messageSender;
    }

    public async Task<Result> StartCreate(CreateCollaboratorTempDTO createDTO)
    {
        try
        {
            await _messageSender.SendCollaboratorTempCreationCommandAsync(createDTO.PeriodDateTime, createDTO.Names, createDTO.Surnames, createDTO.Email, createDTO.FinalDate);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.InternalServerError(ex.Message));
        }
    }

    public async Task<Result<CreatedCollaboratorTempDTO>> Create(PeriodDateTime periodDateTime, string names, string surnames, string email, DateTime finalDate)
    {
        ICollaboratorTemp collaboratorTemp;
        try
        {
            collaboratorTemp = _tempFactory.Create(periodDateTime, names, surnames, email, finalDate);
            collaboratorTemp = await _tempRepository.AddAsync(collaboratorTemp);

            var result = new CreatedCollaboratorTempDTO(collaboratorTemp.Id, periodDateTime, names, surnames, email, finalDate);

            return Result<CreatedCollaboratorTempDTO>.Success(result);
        } catch(Exception ex)
        {
            return Result<CreatedCollaboratorTempDTO>.Failure(Error.InternalServerError(ex.Message));
        }
    }

    public async Task<Result> Remove(Guid collabId)
    {
        ICollaboratorTemp? temp;
        try
        {
            temp = await _tempRepository.GetByIdAsync(collabId);

            if (temp == null)
                return Result.Failure(Error.InternalServerError("Id doesn't exist!"));

            await _tempRepository.RemoveAsync(temp);
            return Result.Success();
        } catch (Exception e)
        {
            return Result.Failure(Error.InternalServerError(e.Message));
        }
    }
}
