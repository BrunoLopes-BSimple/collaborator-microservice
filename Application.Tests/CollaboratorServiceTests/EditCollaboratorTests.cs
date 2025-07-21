using Application.DTO.Collaborators;
using Application.IPublishers;
using Application.ISender;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.CollaboratorServiceTests;

public class EditCollaboratorTests
{
    [Fact]
    public async Task EditCollaborator_ShouldUpdateAndPublishEvent_WhenCollaboratorExists()
    {
        // Arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();
        var publisherDouble = new Mock<IMessagePublisher>();
        var senderDouble = new Mock<IMessageSender>();

        var collabId = Guid.NewGuid();
        var newPeriod = new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(2));
        var dto = new CollabData(collabId, newPeriod);

        var collaborator = new Collaborator(collabId, Guid.NewGuid(), newPeriod);

        collabRepoDouble.Setup(r => r.GetByIdAsync(collabId)).ReturnsAsync(collaborator);
        collabRepoDouble.Setup(r => r.UpdateCollaborator(It.IsAny<ICollaborator>())).ReturnsAsync(collaborator);

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, publisherDouble.Object, senderDouble.Object);

        // Act
        var result = await service.EditCollaborator(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(newPeriod, collaborator.PeriodDateTime);

        collabRepoDouble.Verify(r => r.UpdateCollaborator(It.IsAny<ICollaborator>()), Times.Once);
        publisherDouble.Verify(p => p.PublishCollaboratorUpdatedAsync(collaborator), Times.Once);
    }

    [Fact]
    public async Task EditCollaborator_ShouldReturnNotFoundFailure_WhenCollaboratorDoesNotExist()
    {
        // Arrange
        var collabRepoDouble = new Mock<ICollaboratorRepository>();
        var collabFactoryDouble = new Mock<ICollaboratorFactory>();
        var publisherDouble = new Mock<IMessagePublisher>();
        var senderDouble = new Mock<IMessageSender>();

        var dto = new CollabData(Guid.NewGuid(), new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1)));

        collabRepoDouble.Setup(r => r.GetByIdAsync(dto.Id)).ReturnsAsync((ICollaborator)null);

        var service = new CollaboratorService(collabRepoDouble.Object, collabFactoryDouble.Object, publisherDouble.Object, senderDouble.Object);

        // Act
        var result = await service.EditCollaborator(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Collaborator not found.", result.Error.Message);

        publisherDouble.Verify(p => p.PublishCollaboratorUpdatedAsync(It.IsAny<ICollaborator>()), Times.Never);
    }

}