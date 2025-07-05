using Domain.IRepository;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DataModel;
using Domain.Visitor;
using AutoMapper;

namespace Infrastructure.Repositories;

public class CollaboratorWithoutUserRepositoryEF : GenericRepositoryEF<ICollaboratorWithoutUser, CollaboratorWithoutUser, CollaboratorWithoutUserDataModel>, ICollaboratorWithoutUserRepository
{
    private readonly IMapper _mapper;
    public CollaboratorWithoutUserRepositoryEF(AbsanteeContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
    }

    public async Task<bool> AlreadyExistsAsync(Guid collbId)
    {
        return await _context.Set<CollaboratorWithoutUserDataModel>().AnyAsync(c => c.Id == collbId);
    }

    public override ICollaboratorWithoutUser? GetById(Guid id)
    {
        var collabDM = this._context.Set<CollaboratorWithoutUserDataModel>()
                            .FirstOrDefault(c => c.Id == id);

        if (collabDM == null)
            return null;

        var collab = _mapper.Map<CollaboratorWithoutUserDataModel, CollaboratorWithoutUser>(collabDM);
        return collab;
    }

    public override async Task<ICollaboratorWithoutUser?> GetByIdAsync(Guid id)
    {
        var collabDM = await this._context.Set<CollaboratorWithoutUserDataModel>()
                            .FirstOrDefaultAsync(c => c.Id == id);

        if (collabDM == null)
            return null;

        var collab = _mapper.Map<CollaboratorWithoutUserDataModel, CollaboratorWithoutUser>(collabDM);
        return collab;
    }

    public async Task<IEnumerable<ICollaboratorWithoutUser>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        var collabsDm = await this._context.Set<CollaboratorWithoutUserDataModel>()
                    .Where(c => ids.Contains(c.Id))
                    .ToListAsync();

        var collabs = collabsDm.Select(c => _mapper.Map<CollaboratorWithoutUserDataModel, CollaboratorWithoutUser>(c));

        return collabs;
    }

    public async Task<IEnumerable<ICollaboratorWithoutUser>> GetByUsersIdsAsync(IEnumerable<Guid> ids)
    {
        var collabsDm = await this._context.Set<CollaboratorWithoutUserDataModel>()
                    .Where(c => ids.Contains(c.Id))
                    .ToListAsync();

        var collabs = collabsDm.Select(c => _mapper.Map<CollaboratorWithoutUserDataModel, CollaboratorWithoutUser>(c));

        return collabs;
    }

    public async Task<ICollaboratorWithoutUser?> GetByNameSurnameEmailAsync(string names, string surnames, string email)
    {
        var collabDM = await _context.Set<CollaboratorWithoutUserDataModel>()
                            .FirstOrDefaultAsync(c => c.Names == names &&
                                                c.Surnames == surnames &&
                                                c.Email == email);

        if (collabDM == null)
            return null;

        var collab = _mapper.Map<CollaboratorWithoutUserDataModel, CollaboratorWithoutUser>(collabDM);
        return collab;
    }

}