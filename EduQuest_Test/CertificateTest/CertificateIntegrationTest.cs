using EduQuest_Domain.Models.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Test.UserTest;

public class CertificateIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    //private readonly HttpClient _client;
    //private readonly ITestOutputHelper _outputHelper;

    //public CertificateIntegrationTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    //{
    //    _client = factory.CreateClient();
    //    _outputHelper = output;
    //}

    //[Fact]
    //public async Task Get_ShouldReturnListMappedCertificate()
    //{
    //    //Arange
    //    var queryParams = new Dictionary<string, string>
    //    {
    //        { "pageNo", "1" },
    //        { "eachPage", "2" }
    //    };
    //    var requestUrl = QueryHelpers.AddQueryString("/v1/certificate/filter", queryParams!);


    //    // Act
    //    var response = await _client.GetAsync(requestUrl);
    //    await ResponseFormatter.FormatResponse(response, _outputHelper);

    //    // Assert
    //    response.StatusCode.Should().Be(HttpStatusCode.OK);

    //    var result = await response.Content.ReadFromJsonAsync<APIResponse>();
    //    result.Should().NotBeNull();
    //    result.IsError.Should().BeFalse();
    //    result.Payload.Should().NotBeNull();
    //    result.Message.content.Should().Be(MessageCommon.GetSuccesfully);
    //}


    //[Fact]
    //public async Task Create_ShouldReturnNotFound_WhenLearnerDoesntExist()
    //{
    //    // Arrange: Tạo request tạo chứng chỉ
    //    var createCommand = new
    //    {
    //        Title = "Certificate Test",
    //        Url = "http://example.com/certificate.pdf",
    //        UserId = "user-123",
    //        CourseId = "course-456"
    //    };

    //    // Act: 
    //    var response = await _client.PostAsJsonAsync("/v1/certificate", createCommand);
    //    await ResponseFormatter.FormatResponse(response, _outputHelper);

    //    // Assert
    //    var result = await response.Content.ReadFromJsonAsync<APIResponse>();
    //    result.IsError.Should().BeTrue();
    //    result.Payload.Should().BeNull();
    //    result.Errors.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    //    result.Message.content.Should().Be(MessageCommon.NotFound);
    //}

}
