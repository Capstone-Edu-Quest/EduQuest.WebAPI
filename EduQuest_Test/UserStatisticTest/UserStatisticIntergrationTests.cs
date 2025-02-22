using EduQuest_Application.UseCases.UserStatistics.Commands.UpdateUsersStreak;
using EduQuest_Domain.Models.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Test.UserStatisticTest;

public class UserStatisticIntergrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UserStatisticIntergrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GivenUnexistedUser_ShouldNotUpdate()
    {
        //Arrange
        var command = new UpdateUsersStreakCommand { UserId = "test" };

        //Act
        var response = await _client.PutAsJsonAsync("/v1/userStatistic/streak", command);


        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<APIResponse>();
        result!.IsError.Should().BeTrue();
        result.Message!.content.Should().Be(MessageCommon.NotFound);
    }
}