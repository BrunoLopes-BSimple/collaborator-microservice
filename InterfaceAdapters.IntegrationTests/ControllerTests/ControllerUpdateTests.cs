using Application.DTO.Collaborators;
using Domain.Models;
using Infrastructure;
using Infrastructure.DataModel;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InterfaceAdapters.IntegrationTests.ControllerTests;

public class ControllerUpdateTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;
    public ControllerUpdateTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    { _factory = factory; }

    [Fact]
    public async Task Update_ShouldUpdateCollaboratorPeriod_ThenReturnsSuccess()
    {
        // arrange
        var collabId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var period = new PeriodDateTime(DateTime.Now.AddDays(5).ToUniversalTime(), DateTime.Now.AddDays(100).ToUniversalTime());

        await using (var scope = _factory.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AbsanteeContext>();

            context.Collaborators.Add(new CollaboratorDataModel(collabId, userId, period));
            await context.SaveChangesAsync();
        }

        var newPeriod = new PeriodDateTime(DateTime.Now.AddDays(20).ToUniversalTime(), DateTime.Now.AddDays(120).ToUniversalTime());
        var payload = new CollabDetailsDTO(collabId, newPeriod);

        // act
        var response = await PutAndDeserializeAsync<CollabUpdatedDTO>("api/collaborators", payload);

        // assert
        Assert.NotNull(response);
        Assert.Equal(newPeriod, response.PeriodDateTime);
    }
}
