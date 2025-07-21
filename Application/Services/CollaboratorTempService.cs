using Application.DTO.Collaborators;
using Application.Interfaces;
using Application.ISender;
using Domain.Factory.CollabWithoutUserFactory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;

namespace Application.Services
{
    public class CollaboratorTempService : ICollaboratorTempService
    {
        private readonly ICollaboratorWithoutUserRepository _tempRepo;
        private readonly ICollaboratorWithoutUserFactory _tempFactory;
        private readonly IMessageSender _sender;

        public CollaboratorTempService(ICollaboratorWithoutUserRepository tempRepo, ICollaboratorWithoutUserFactory tempFactory, IMessageSender sender)
        {
            _tempRepo = tempRepo;
            _tempFactory = tempFactory;
            _sender = sender;
        }

        public async Task<Result<CreatedCollaboratorTempDTO>> Create(PeriodDateTime periodDateTime, string names, string surnames, string email, DateTime finalDate)
        {
            ICollaboratorWithoutUser collaboratorTemp;
            try
            {
                var collabDM = new CollaboratorWithoutUserDataModel { Id = Guid.NewGuid(), Names = names, Surnames = surnames, Email = email, DeactivationDate = finalDate, PeriodDateTime = periodDateTime };

                collaboratorTemp = _tempFactory.Create(collabDM);
                collaboratorTemp = await _tempRepo.AddAsync(collaboratorTemp);

                var result = new CreatedCollaboratorTempDTO(collaboratorTemp.Id, periodDateTime, names, surnames, email, finalDate);

                return Result<CreatedCollaboratorTempDTO>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<CreatedCollaboratorTempDTO>.Failure(Error.InternalServerError(ex.Message));
            }
        }
    }
}