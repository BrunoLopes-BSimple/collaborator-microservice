using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;
using Application.IPublishers;
using Application.ISender;

namespace Application.Tests.CollaboratorServiceTests;

public class AddCollaboratorReferenceAsyncTests
{
    [Fact]
    public async Task AddCollaboratorReferenceAsync_ShouldCreateAndAddCollaborator_WhenCollaboratorDoesNotExist()
    {
        // Arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();
        var publisherDouble = new Mock<IMessagePublisher>();
        var senderDouble = new Mock<IMessageSender>();

        var collabId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1));

        var collab = new Collaborator(collabId, userId, period);

        collabRepoDouble.Setup(r => r.AlreadyExistsAsync(collabId)).ReturnsAsync(false);
        collabFactoryDouble.Setup(f => f.Create(collabId, userId, period)).Returns(collab);
        collabRepoDouble.Setup(r => r.AddAsync(collab)).ReturnsAsync(collab);

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, publisherDouble.Object, senderDouble.Object);

        // Act
        var result = await service.AddCollaboratorReferenceAsync(collabId, userId, period);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(collab, result);

        collabRepoDouble.Verify(r => r.AlreadyExistsAsync(collabId), Times.Once);
        collabFactoryDouble.Verify(f => f.Create(collabId, userId, period), Times.Once);
        collabRepoDouble.Verify(r => r.AddAsync(collab), Times.Once);
    }

    [Fact]
    public async Task AddCollaboratorReferenceAsync_ShouldReturnNull_WhenCollaboratorAlreadyExists()
    {
        // Arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();
        var publisherDouble = new Mock<IMessagePublisher>();
        var senderDouble = new Mock<IMessageSender>();

        var collabId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1));

        collabRepoDouble.Setup(r => r.AlreadyExistsAsync(collabId)).ReturnsAsync(true);

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, publisherDouble.Object, senderDouble.Object);

        // Act
        var result = await service.AddCollaboratorReferenceAsync(collabId, userId, period);

        // Assert
        Assert.Null(result);

        collabFactoryDouble.Verify(f => f.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDateTime>()), Times.Never);
        collabRepoDouble.Verify(r => r.AddAsync(It.IsAny<ICollaborator>()), Times.Never);
    }

}