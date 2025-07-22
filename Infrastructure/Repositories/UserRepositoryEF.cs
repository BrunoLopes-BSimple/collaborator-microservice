using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepositoryEF : GenericRepositoryEF<IUser, User, UserDataModel>, IUserRepository
{
    private readonly IMapper _mapper;
    public UserRepositoryEF(AbsanteeContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
    } 

    public override IUser? GetById(Guid id)
    {
        var userDM = _context.Set<UserDataModel>().FirstOrDefault(c => c.Id == id);

        if (userDM == null)
            return null;

        var user = _mapper.Map<UserDataModel, User>(userDM);
        return user;
    }

    public override async Task<IUser?> GetByIdAsync(Guid id)
    {
        var userDM = await _context.Set<UserDataModel>().FirstOrDefaultAsync(u => u.Id == id);

        if (userDM == null)
            return null;

        var user = _mapper.Map<UserDataModel, User>(userDM);
        return user;
    }

    public async Task<bool> Exists(Guid ID)
    {
        var userDM = await _context.Set<UserDataModel>().FirstOrDefaultAsync(u => u.Id == ID);
        if (userDM == null)
            return false;
        return true;
    }
}
