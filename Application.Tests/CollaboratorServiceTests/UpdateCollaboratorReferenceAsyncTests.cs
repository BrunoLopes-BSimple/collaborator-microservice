/* using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IPublishers;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.CollaboratorServiceTests
{
    public class UpdateCollaboratorReferenceAsyncTests
    {
        [Fact]
        public async Task UpdateCollaboratorReferenceAsync_ShouldUpdateAndReturnCollaborator_WhenCollaboratorExists()
        {
            // Arrange
            var collabRepoDouble = new Mock<ICollaboratorRepository>();
            var collabFactoryDouble = new Mock<ICollaboratorFactory>();
            var publisherDouble = new Mock<IMessagePublisher>();

            var collabId = Guid.NewGuid();
            var newPeriod = new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(5));

            var fakeCollaborator = new Collaborator(collabId, Guid.NewGuid(), new PeriodDateTime(DateTime.Now, DateTime.Now));

            collabRepoDouble.Setup(r => r.GetByIdAsync(collabId)).ReturnsAsync(fakeCollaborator);
            collabRepoDouble.Setup(r => r.UpdateCollaborator(fakeCollaborator)).ReturnsAsync(fakeCollaborator);

            var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, publisherDouble.Object);

            // Act
            var result = await service.UpdateCollaboratorReferenceAsync(collabId, Guid.NewGuid(), newPeriod);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fakeCollaborator, result);

            collabRepoDouble.Verify(r => r.GetByIdAsync(collabId), Times.Once);
            collabRepoDouble.Verify(r => r.UpdateCollaborator(fakeCollaborator), Times.Once);
        }

        [Fact]
        public async Task UpdateCollaboratorReferenceAsync_ShouldReturnNull_WhenCollaboratorNotFound()
        {
            // Arrange
            var collabRepoDouble = new Mock<ICollaboratorRepository>();
            var collabFactoryDouble = new Mock<ICollaboratorFactory>();
            var publisherDouble = new Mock<IMessagePublisher>();

            var collabId = Guid.NewGuid();

            collabRepoDouble.Setup(r => r.GetByIdAsync(collabId)).ReturnsAsync((ICollaborator)null);

            var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, publisherDouble.Object);

            // Act
            var result = await service.UpdateCollaboratorReferenceAsync(collabId, Guid.NewGuid(), new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1)));

            // Assert
            Assert.Null(result);
            collabRepoDouble.Verify(r => r.UpdateCollaborator(It.IsAny<ICollaborator>()), Times.Never);
        }
    }
} */