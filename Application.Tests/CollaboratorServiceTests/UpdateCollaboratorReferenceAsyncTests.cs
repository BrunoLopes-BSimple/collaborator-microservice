using Application.IPublishers;
using Application.ISender;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.CollaboratorServiceTests;

public class UpdateCollaboratorReferenceAsyncTests
{
    [Fact]
    public async Task UpdateCollaboratorReferenceAsync_ShouldUpdateAndReturnCollaborator_WhenCollaboratorExists()
    {
        // Arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();
        var publisherDouble = new Mock<IMessagePublisher>();
        var senderDouble = new Mock<IMessageSender>();

        var collabId = Guid.NewGuid();
        var newPeriod = new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(5));

        var collaborator = new Collaborator(collabId, Guid.NewGuid(), new PeriodDateTime(DateTime.Now, DateTime.Now));

        collabRepoDouble.Setup(r => r.GetByIdAsync(collabId)).ReturnsAsync(collaborator);
        collabRepoDouble.Setup(r => r.UpdateCollaborator(collaborator)).ReturnsAsync(collaborator);

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, publisherDouble.Object, senderDouble.Object);

        // Act
        var result = await service.UpdateCollaboratorReferenceAsync(collabId, Guid.NewGuid(), newPeriod);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(collaborator, result);
        Assert.Equal(newPeriod, collaborator.PeriodDateTime);

        collabRepoDouble.Verify(r => r.GetByIdAsync(collabId), Times.Once);
        collabRepoDouble.Verify(r => r.UpdateCollaborator(collaborator), Times.Once);
    }

    [Fact]
    public async Task UpdateCollaboratorReferenceAsync_ShouldReturnNull_WhenCollaboratorNotFound()
    {
        // Arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();
        var publisherDouble = new Mock<IMessagePublisher>();
        var senderDouble = new Mock<IMessageSender>();

        var collabId = Guid.NewGuid();

        collabRepoDouble.Setup(r => r.GetByIdAsync(collabId)).ReturnsAsync((ICollaborator)null);

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, publisherDouble.Object, senderDouble.Object);

        // Act
        var result = await service.UpdateCollaboratorReferenceAsync(collabId, Guid.NewGuid(), new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1)));

        // Assert
        Assert.Null(result);
        collabRepoDouble.Verify(r => r.UpdateCollaborator(It.IsAny<ICollaborator>()), Times.Never);

    }
}