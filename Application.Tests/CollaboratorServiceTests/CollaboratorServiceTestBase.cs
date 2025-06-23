/* using Application.Messaging;
using Application.Services;
using AutoMapper;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.Tests.CollaboratorServiceTests
{
    public abstract class CollaboratorServiceTestBase : IDisposable
    {
        protected AbsanteeContext Context;
        protected ICollaboratorRepository CollaboratorRepository;
        protected Mock<ICollaboratorFactory> CollaboratorFactoryMock;
        protected Mock<IMessagePublisher> MessagePublisherMock;
        protected CollaboratorService Service;
        protected Mock<IMapper> MapperMock; 

        protected CollaboratorServiceTestBase()
        {
            var options = new DbContextOptionsBuilder<AbsanteeContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            Context = new AbsanteeContext(options);

            MapperMock = new Mock<IMapper>();

            MapperMock.Setup(m => m.Map<Collaborator, CollaboratorDataModel>(It.IsAny<Collaborator>()))
                      .Returns((Collaborator c) => new CollaboratorDataModel
                      {
                          Id = c.Id,
                          UserId = c.UserId,
                          PeriodDateTime = c.PeriodDateTime
                      });

            MapperMock.Setup(m => m.Map<CollaboratorDataModel, Collaborator>(It.IsAny<CollaboratorDataModel>()))
                      .Returns((CollaboratorDataModel dm) => new Collaborator(dm.Id, dm.UserId, dm.PeriodDateTime));


            CollaboratorRepository = new CollaboratorRepositoryEF(Context, MapperMock.Object);

            CollaboratorFactoryMock = new Mock<ICollaboratorFactory>();
            MessagePublisherMock = new Mock<IMessagePublisher>();

            Service = new CollaboratorService(
                CollaboratorRepository,
                CollaboratorFactoryMock.Object,
                MessagePublisherMock.Object
            );
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
} */