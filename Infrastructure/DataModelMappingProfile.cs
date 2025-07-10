using AutoMapper;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Resolvers;

namespace Infrastructure;

public class DataModelMappingProfile : Profile
{
    public DataModelMappingProfile()
    {
        CreateMap<Collaborator, CollaboratorDataModel>();
        CreateMap<CollaboratorDataModel, Collaborator>().ConvertUsing<CollaboratorDataModelConverter>();

        CreateMap<User, UserDataModel>();
        CreateMap<UserDataModel, User>().ConvertUsing<UserDataModelConverter>();

        CreateMap<CollaboratorWithoutUser, CollaboratorWithoutUserDataModel>();
        CreateMap<CollaboratorWithoutUserDataModel, CollaboratorWithoutUser>().ConvertUsing<CollaboratorWithoutUserDataModelConverter>();
    }

}