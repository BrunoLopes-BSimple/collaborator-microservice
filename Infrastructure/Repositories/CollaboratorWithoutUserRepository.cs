using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CollaboratorWithoutUserRepository : GenericRepositoryEF<ICollaboratorWithoutUser, CollaboratorWithoutUser, CollaboratorWithoutUserDataModel>, ICollaboratorWithoutUserRepository
    {
        private readonly IMapper _mapper;
        public CollaboratorWithoutUserRepository(AbsanteeContext context, IMapper mapper) : base(context, mapper)
        {
            _mapper = mapper;
        }

        public override ICollaboratorWithoutUser? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override async Task<ICollaboratorWithoutUser?> GetByIdAsync(Guid id)
        {
            var collabDM = await this._context.Set<CollaboratorWithoutUserDataModel>().FirstOrDefaultAsync(c => c.Id == id);

            if (collabDM == null)
                return null;

            var collab = _mapper.Map<CollaboratorWithoutUserDataModel, CollaboratorWithoutUser>(collabDM);
            return collab;
        }

        public async Task DeleteAsync(Guid id)
        {
            var collabDM = await this._context.Set<CollaboratorWithoutUserDataModel>().FirstOrDefaultAsync(c => c.Id == id);

            if (collabDM == null)
                return;

            this._context.Set<CollaboratorWithoutUserDataModel>().Remove(collabDM);
            await this._context.SaveChangesAsync();
        }

    }
}