using Application.IPublishers;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;
using Application.DTO;

namespace Application.Tests.CollaboratorServiceTests
{
    public class CreateCollaboratorTests
    {
        [Fact]
        public async Task Create_ShouldCreateCollaboratorAndPublishMessage_WhenPassingValidData()
        {
            // Arrange
            var collabRepoDouble = new Mock<ICollaboratorRepository>();
            var collabFactoryDouble = new Mock<ICollaboratorFactory>();
            var publisherDouble = new Mock<IMessagePublisher>();

            var userId = Guid.NewGuid();
            var collabId = Guid.NewGuid();
            var period = new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1));
            var collab = new Collaborator(collabId, userId, period);


            collabFactoryDouble.Setup(f => f.Create(userId, period)).ReturnsAsync(collab);
            collabRepoDouble.Setup(cr => cr.AddAsync(It.IsAny<ICollaborator>())).ReturnsAsync(collab);

            var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, publisherDouble.Object);
            var createDto = new CreateCollaboratorDTO(userId, period);

            // Act
            var result = await service.Create(createDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);

            Assert.Equal(collabId, result.Value.CollaboratorId);
            Assert.Equal(userId, result.Value.UserId);
            Assert.Equal(period, result.Value.PeriodDateTime);

            publisherDouble.Verify(p => p.PublishCollaboratorCreatedAsync(collab), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldReturnFailureResult_WhenFactoryThrowsException()
        {
            // Arrange
            var collabRepoDouble = new Mock<ICollaboratorRepository>();
            var collabFactoryDouble = new Mock<ICollaboratorFactory>();
            var publisherDouble = new Mock<IMessagePublisher>();

            var createDto = new CreateCollaboratorDTO(
                Guid.NewGuid(),
                new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1))
            );

            var expectedException = new ArgumentException("Simulated factory error");

            collabFactoryDouble.Setup(f => f.Create(createDto.UserId, createDto.PeriodDateTime)).ThrowsAsync(expectedException);

            var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, publisherDouble.Object);


            // Act
            var result = await service.Create(createDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal(result.Error.Message, expectedException.Message);

            collabRepoDouble.Verify(r => r.AddAsync(It.IsAny<ICollaborator>()), Times.Never);
            publisherDouble.Verify(p => p.PublishCollaboratorCreatedAsync(It.IsAny<ICollaborator>()), Times.Never);
        }
    }
}