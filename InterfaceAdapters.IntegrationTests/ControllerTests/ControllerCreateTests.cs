using System.Net;
using Application.DTO;
using Application.DTO.Collaborators;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ControllerTests;

public class ControllerCreateTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;
    public ControllerCreateTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    { _factory = factory; }

    [Fact]
    public async Task Create_CreateWithUserData_ShouldReturnSuccess()
    {
        // arrange
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.Now.AddDays(5).ToUniversalTime(), DateTime.Now.AddDays(100).ToUniversalTime());
        var names = "name";
        var surnames = "surname";
        var email = "email@gmail.com";
        var finalDate = DateTime.Now.AddYears(1).ToUniversalTime();
        var createCollabDto = new CreateCollabDTO(userId, period, names, surnames, email, finalDate);

        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AbsanteeContext>();

            context.ValidUserIds.Add(new UserDataModel(userId));
            await context.SaveChangesAsync();
        }

        // act
        var response = await PostAndDeserializeAsync<CreatedCollaboratorDTO>("api/collaborators", createCollabDto);

        // assert
        Assert.NotNull(response);
        Assert.Equal(userId, response.UserId);
        Assert.NotEqual(Guid.Empty, response.CollaboratorId);
        Assert.Equal(period, response.PeriodDateTime);
    }

    [Fact]
    public async Task Create_CreateWithoutUserData_ShouldReturnSuccess()
    {
        // arrange
        var period = new PeriodDateTime(DateTime.Now.AddDays(5).ToUniversalTime(), DateTime.Now.AddDays(100).ToUniversalTime());
        var names = "name";
        var surnames = "surname";
        var email = "email@gmail.com";
        var finalDate = DateTime.Now.AddYears(1).ToUniversalTime();
        var createCollabDto = new CreateCollabDTO(null, period, names, surnames, email, finalDate);

        // act - utilizado pela saga, por isso não devolve nada
        var response = await PostAsync("api/collaborators", createCollabDto);

        // assert
        Assert.NotNull(response);
    }

    [Fact]
    public async Task Create_CreateWithoutUserDataAndIncorrectParams_ShouldReturnBadRequest()
    {
        // arrange
        var period = new PeriodDateTime(DateTime.Now.AddDays(5).ToUniversalTime(), DateTime.Now.AddDays(100).ToUniversalTime());
        var names = "name";
        var surnames = "surname";
        var email = "email@gmail.com";
        var finalDate = DateTime.Now.AddYears(-3).ToUniversalTime();
        var createCollabDto = new CreateCollabDTO(null, period, names, surnames, email, finalDate);

        // act 
        var response = await PostAsync("api/collaborators", createCollabDto);

        // assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("Missing required fields for temp collaborator creation.", body);
    }
}
