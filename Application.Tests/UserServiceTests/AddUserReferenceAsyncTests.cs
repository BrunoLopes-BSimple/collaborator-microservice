using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.Tests.UserServiceTests
{
    public class AddUserReferenceAsyncTests
    {
        [Fact]
        public async Task AddUserReferenceAsync_ShouldAddUser_WhenFactoryCreatesUser()
        {
            // Arrange
            var userRepoDouble = new Mock<IUserRepository>();
            var userFactoryDouble = new Mock<IUserFactory>();

            var userId = Guid.NewGuid();
            var user = new User(userId);

            userFactoryDouble.Setup(f => f.Create(userId)).ReturnsAsync(user);
            userRepoDouble.Setup(r => r.AddAsync(user)).ReturnsAsync(user);

            var service = new UserService(userRepoDouble.Object, userFactoryDouble.Object);

            // Act
            var result = await service.AddUserReferenceAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user, result);

            userFactoryDouble.Verify(f => f.Create(userId), Times.Once);
            userRepoDouble.Verify(r => r.AddAsync(user), Times.Once);
        }

        [Fact]
        public async Task AddUserReferenceAsync_ShouldReturnNull_WhenFactoryReturnsNull()
        {
            // Arrange
            var userRepoDouble = new Mock<IUserRepository>();
            var userFactoryDouble = new Mock<IUserFactory>();

            var userId = Guid.NewGuid();

            userFactoryDouble.Setup(f => f.Create(userId)).ReturnsAsync((User)null);

            var service = new UserService(userRepoDouble.Object, userFactoryDouble.Object);

            // Act
            var result = await service.AddUserReferenceAsync(userId);

            // Assert
            Assert.Null(result);

            userRepoDouble.Verify(r => r.AddAsync(It.IsAny<IUser>()), Times.Never);
        }
    }
}