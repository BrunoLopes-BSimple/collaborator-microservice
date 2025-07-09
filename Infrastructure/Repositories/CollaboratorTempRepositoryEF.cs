using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CollaboratorTempRepositoryEF : GenericRepositoryEF<ICollaboratorTemp, CollaboratorTemp, CollaboratorTempDataModel>, ICollaboratorTempRepository
{
    private readonly IMapper _mapper;

    public CollaboratorTempRepositoryEF(AbsanteeContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
    }

    public override ICollaboratorTemp? GetById(Guid id)
    {
        var collabDM = _context.Set<CollaboratorTempDataModel>()
                            .FirstOrDefault(c => c.Id == id);

        if (collabDM == null)
            return null;

        var collab = _mapper.Map<CollaboratorTempDataModel, ICollaboratorTemp>(collabDM);
        return collab;
    }

    public override async Task<ICollaboratorTemp?> GetByIdAsync(Guid id)
    {
        var collabDM = await _context.Set<CollaboratorTempDataModel>()
                            .FirstOrDefaultAsync(c => c.Id == id);

        if (collabDM == null)
            return null;

        var collab = _mapper.Map<CollaboratorTempDataModel, ICollaboratorTemp>(collabDM);
        return collab;
    }
}
