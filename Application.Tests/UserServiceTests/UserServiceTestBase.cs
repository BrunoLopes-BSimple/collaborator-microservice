/* using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using AutoMapper;
using Domain.IRepository;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.Tests.UserServiceTests
{
    public abstract class UserServiceTestBase : IDisposable
    {
        protected IUserRepository UserRepository;
        protected UserService UserService;
        protected AbsanteeContext Context;
        protected Mock<IMapper> MapperMock;

        protected UserServiceTestBase()
        {
            var options = new DbContextOptionsBuilder<AbsanteeContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            Context = new AbsanteeContext(options);
            MapperMock = new Mock<IMapper>();

            MapperMock.Setup(m => m.Map<UserDataModel, User>(It.IsAny<UserDataModel>()))
                      .Returns((UserDataModel dm) => new User(dm.Id));

            MapperMock.Setup(m => m.Map<User, UserDataModel>(It.IsAny<User>()))
                      .Returns((User u) => new UserDataModel { Id = u.Id });

            UserRepository = new UserRepositoryEF(Context, MapperMock.Object);
            UserService = new UserService(UserRepository);
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
} */