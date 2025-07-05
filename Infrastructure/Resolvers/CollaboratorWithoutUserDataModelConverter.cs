using AutoMapper;
using Domain.Factory;
using Domain.Models;
using Infrastructure.DataModel;

namespace Infrastructure.Resolvers;

public class CollaboratorWithoutUserDataModelConverter : ITypeConverter<CollaboratorWithoutUserDataModel, CollaboratorWithoutUser>
{
    private readonly ICollaboratorWithoutUserFactory _factory;

    public CollaboratorWithoutUserDataModelConverter(ICollaboratorWithoutUserFactory factory)
    {
        _factory = factory;
    }

    public CollaboratorWithoutUser Convert(CollaboratorWithoutUserDataModel source, CollaboratorWithoutUser destination, ResolutionContext context)
    {
        return _factory.Create(source);
    }
}