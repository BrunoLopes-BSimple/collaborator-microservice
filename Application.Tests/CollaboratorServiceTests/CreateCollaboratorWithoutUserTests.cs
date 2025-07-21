using Application.DTO.Collaborators;
using Application.IPublishers;
using Application.ISender;
using Application.Services;
using Domain.Contracts;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Application.Tests.CollaboratorServiceTests;
    public class CreateCollaboratorWithoutUserTests
    {
    [Fact]
    public async Task CreateCollaboratorWithoutUser_ShouldSendMessageAndReturnSuccess()
    {
        // Arrange
        var collabRepo = new Mock<ICollaboratorRepository>();
        var collabFactory = new Mock<ICollaboratorFactory>();
        var publisher = new Mock<IMessagePublisher>();
        var sender = new Mock<IMessageSender>();

        var service = new CollaboratorService(collabRepo.Object, collabFactory.Object, publisher.Object, sender.Object);

        var dto = new CollabWithoutUserDTO(
            "Some", "Random", "name@example.com",
            DateTime.MaxValue,
            new PeriodDateTime(DateTime.Today, DateTime.Today.AddDays(10))
        );

        // Act
        var result = await service.CreateCollaboratorWithoutUser(dto);

        // Assert
        Assert.True(result.IsSuccess);
        sender.Verify(s => s.SendCollaboratorCreationCommandAsync(It.IsAny<CreateCollaboratorCommand>()), Times.Once);
    }

    [Fact]
    public async Task CreateCollaboratorWithoutUser_ShouldReturnFailure_WhenArgumentExceptionThrown()
    {
        // Arrange
        var collabRepo = new Mock<ICollaboratorRepository>();
        var collabFactory = new Mock<ICollaboratorFactory>();
        var publisher = new Mock<IMessagePublisher>();
        var sender = new Mock<IMessageSender>();

        sender
            .Setup(s => s.SendCollaboratorCreationCommandAsync(It.IsAny<CreateCollaboratorCommand>()))
            .ThrowsAsync(new ArgumentException("Invalid email"));

        var service = new CollaboratorService(collabRepo.Object, collabFactory.Object, publisher.Object, sender.Object);

        var dto = new CollabWithoutUserDTO(
            "Some", "Random", "name@example.com",
            DateTime.MaxValue,
            new PeriodDateTime(DateTime.Today, DateTime.Today.AddDays(10))
        );

        // Act
        var result = await service.CreateCollaboratorWithoutUser(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Invalid email", result.Error.Message);
    }

}
