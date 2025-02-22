using EduQuest_Application.DTO.Response.Badges;
using EduQuest_Application.UseCases.Badges.Commands.CreateBadge;
using EduQuest_Application.UseCases.Badges.Commands.UpdateBadge;
using EduQuest_Domain.Models.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using static EduQuest_Domain.Constants.Constants;


namespace EduQuest_Test.BadgeTest;

public class BadgeIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BadgeIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GivenValidCommand_ShouldCreateBadgeSuccessfully()
    {
        var command = new CreateBadgeCommand
        {
            Name = "Integration Test Badge",
            Description = "Integration Test Description",
            IconUrl = "http://example.com/icon.png",
            Color = "#FF5733"
        };

        var response = await _client.PostAsJsonAsync("/v1/badges", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<APIResponse>();
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Message.content.Should().Be(MessageCommon.CreateSuccesfully);
    }


    [Fact]
    public async Task GivenExistingBadges_ShouldReturnBadgesList()
    {
        //Arange
        var createCommand = new CreateBadgeCommand
        {
            Name = "Test Badge",
            Description = "Description",
            IconUrl = "http://example.com/icon.png",
            Color = "#FF5733"
        };
        await _client.PostAsJsonAsync("/v1/badges", createCommand);

        // Act
        var response = await _client.GetAsync($"/v1/badges/filter?Name={createCommand.Name}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<APIResponse>();

        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Payload.Should().NotBeNull();
        result.Message.content.Should().Be(MessageCommon.GetSuccesfully);
    }


    [Fact]
    public async Task GivenValidCommand_ShouldUpdateBadgeSuccessfully()
    {
        //Arrange
        var createCommand = new CreateBadgeCommand
        {
            Name = "Initial Badge",
            Description = "Initial Description",
            IconUrl = "http://example.com/icon.png",
            Color = "#000000"
        };
        var createResponse = await _client.PostAsJsonAsync("/v1/badges", createCommand);
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdBadge = await createResponse.Content.ReadFromJsonAsync<APIResponse>();
        var badgeDto = createdBadge?.Payload as BadgeDto;
        var badgeId = badgeDto?.Id;
        badgeId.Should().NotBeNullOrEmpty();

        //Act
        var updateCommand = new UpdateBadgeCommand(
            badgeId!,
            "Updated Badge",
            "Updated Description",
            "http://example.com/new-icon.png",
            "#FF5733"
        );

        var updateResponse = await _client.PutAsJsonAsync("/v1/badges", updateCommand);

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updateResult = await updateResponse.Content.ReadFromJsonAsync<APIResponse>();

        updateResult.Should().NotBeNull();
        updateResult.IsError.Should().BeFalse();
        updateResult.Payload.Should().NotBeNull();
        updateResult.Message.content.Should().Be(MessageCommon.UpdateSuccesfully);
    }
}
