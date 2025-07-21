using Application.IPublishers;
using Application.ISender;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure.DataModel;
using Moq;

namespace Application.Tests.CollaboratorServiceTests;

    public class FinalizeAsyncTests
    {
    [Fact]
    public async Task FinalizeAsync_ShouldCreateCollaboratorAndPublishEvent()
    {
        // Arrange
        var collabRepo = new Mock<ICollaboratorRepository>();
        var collabFactory = new Mock<ICollaboratorFactory>();
        var publisher = new Mock<IMessagePublisher>();
        var sender = new Mock<IMessageSender>();

        var collabId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.Today, DateTime.Today.AddDays(10));

        var collab = new Collaborator(collabId, userId, period);

        collabFactory.Setup(f => f.Create(It.IsAny<CollaboratorDataModel>())).Returns(collab);
        collabRepo.Setup(r => r.AddAsync(collab)).ReturnsAsync(collab);

        var service = new CollaboratorService(collabRepo.Object, collabFactory.Object, publisher.Object, sender.Object);

        // Act
        await service.FinalizeAsync(collabId, userId, period);

        // Assert
        collabFactory.Verify(f => f.Create(It.Is<CollaboratorDataModel>(
            m => m.Id == collabId && m.UserId == userId && m.PeriodDateTime == period)), Times.Once);

        collabRepo.Verify(r => r.AddAsync(collab), Times.Once);
        publisher.Verify(p => p.PublishCollaboratorCreatedAsync(collab), Times.Once);
    }

}
