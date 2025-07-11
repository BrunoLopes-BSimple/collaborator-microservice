using Application.DTO;
using Application.DTO.Collaborators;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.Tests;

public class CreateCollaboratorControllerTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;

    public CreateCollaboratorControllerTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateCollaborator_WithUserId_ReturnsCreated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.UtcNow.AddDays(1),DateTime.UtcNow.AddDays(10));

        // Insert user into the test DB
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AbsanteeContext>();

            db.ValidUserIds.Add(new UserDataModel
            {
                Id = userId
            });

            await db.SaveChangesAsync();
        }

        var payload = new CreateCollabDTO
        {
            UserId = userId,
            Names = null,
            Surnames = null,
            Email = null,
            FinalDate = null,
            PeriodDateTime = period
        };

        // Act
        var response = await PostAndDeserializeAsync<CreatedCollaboratorDTO>("/api/collaborators", payload);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(userId, response.UserId);
        Assert.NotEqual(Guid.Empty, response.CollaboratorId);
        Assert.Equal(period._initDate, response.PeriodDateTime._initDate);
        Assert.Equal(period._finalDate, response.PeriodDateTime._finalDate);
    }

    [Fact]
    public async Task CreateCollaborator_WithoutUserId_CreatesTempCollaborator()
    {
        // Arrange
        var period = new PeriodDateTime(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(10));
        var payload = new CreateCollabDTO
        {
            UserId = null,
            Names = "John",
            Surnames = "Doe",
            Email = "john.doe@example.com",
            FinalDate = DateTime.UtcNow.AddYears(1),
            PeriodDateTime = period
        };

        // Act
        var response = await PostAsync("/api/collaborators", payload);

        // Assert
        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    }

    [Fact]
    public async Task CreateCollaborator_MissingFields_ReturnsBadRequest()
    {
        // Arrange

        var payload = new CreateCollabDTO
        {
            UserId = null,
            Names = null,
            Surnames = null,
            Email = null,
            FinalDate = null,
            PeriodDateTime = null
        };

        // Act
        var response = await PostAsync("/api/collaborators", payload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

    }


}
