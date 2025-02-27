using EduQuest_Application.DTO.Response;
using EduQuest_Domain.Models.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit.Abstractions;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Test.UserTest;

public class UserIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _outputHelper;

    public UserIntegrationTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _client = factory.CreateClient();
        _outputHelper = output;
    }

    [Fact]
    public async Task GivenUsersExist_ShouldReturnCorrectlyMappedUserList()
    {
        // Act
        var response = await _client.GetAsync("/v1/user/all");
        await ResponseFormatter.FormatResponse(response, _outputHelper);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<APIResponse>();
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Payload.Should().NotBeNull();
        result.Message.content.Should().Be(MessageCommon.GetSuccesfully);
    }


}
