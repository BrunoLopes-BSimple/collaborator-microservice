using AutoMapper;
using Domain.Factory.CollaboratorTempFactory;
using Domain.Interfaces;
using Infrastructure.DataModel;

namespace Infrastructure.Resolvers;

public class CollaboratorTempDataModelConverter : ITypeConverter<CollaboratorTempDataModel, ICollaboratorTemp>
{
    private readonly ICollaboratorTempFactory _factory;

    public CollaboratorTempDataModelConverter(ICollaboratorTempFactory factory)
    {
        _factory = factory;
    }

    public ICollaboratorTemp Convert(CollaboratorTempDataModel source, ICollaboratorTemp destination, ResolutionContext context)
    {
        return _factory.Create(source);
    }
}