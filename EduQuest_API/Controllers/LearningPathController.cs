using EduQuest_Application.DTO.Request.LearningPaths;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.LearningPaths.Commands.CreateLearningPath;
using EduQuest_Application.UseCases.LearningPaths.Commands.DeleteLearningPath;
using EduQuest_Application.UseCases.LearningPaths.Commands.DuplicateLearningPath;
using EduQuest_Application.UseCases.LearningPaths.Commands.UpdateLearningPath;
using EduQuest_Application.UseCases.LearningPaths.Queries.GetLearningPathDetail;
using EduQuest_Application.UseCases.LearningPaths.Queries.GetMyLearningPaths;
using EduQuest_Application.UseCases.LearningPaths.Queries.GetMyPublicLearningPaths;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace EduQuest_API.Controllers;
[Route(Constants.Http.API_VERSION + "/learningPath")]
public class LearningPathController : Controller
{

    private ISender _mediator;
    public LearningPathController(ISender mediator)
    {
        _mediator = mediator;
    }

    
    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetAllUserLearningPath(
        [FromQuery, AllowNull] string? keyWord, 
        [FromQuery, AllowNull] bool? isPublic,
        [FromQuery, AllowNull] bool? isEnrolled,
        [FromQuery, AllowNull] bool? createdByExpert,
        //[FromQuery] string UserId,
        [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10, CancellationToken cancellationToken = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new GetMyLearningPathQuery(userId, keyWord, isPublic, isEnrolled, createdByExpert, pageNo, eachPage), cancellationToken);
        if((result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK))
        {
            return BadRequest(result);
        }
        PagedList<MyLearningPathResponse> list = (PagedList<MyLearningPathResponse>)result.Payload!;
        Response.Headers.Add("X-Total-Element", list.TotalItems.ToString());
        Response.Headers.Add("X-Total-Page", list.TotalPages.ToString());
        Response.Headers.Add("X-Current-Page", list.CurrentPage.ToString());
        return Ok(result);
    }

    [Authorize]
    [HttpPost("dup")]
    public async Task<IActionResult> DuplicateLearningPath([FromQuery, Required] string learningPathId,
        //[FromQuery] string UserId, 
        CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new DuplicateLearningPathCommand(learningPathId, userId), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetLearningPathDetail([FromQuery] string learningPathId, CancellationToken token = default)
    {
        var result = await _mediator.Send(new GetLearningPathDetailQuery(learningPathId), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }


    [HttpGet("public")]
    public async Task<IActionResult> GetMyPublicLearningPath([FromQuery] string? userId, CancellationToken token = default)
    {
        var result = await _mediator.Send(new GetMyPublicLearningPathQuery(userId), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }

    /*[HttpGet("dup")]
    public async Task<IActionResult> GetMyDuplicatedLearningPath([FromQuery, Required] string UserId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }*/

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateLearningPath([FromBody, Required] CreateLearningPathRequest request,
                                                        //[FromQuery] string userId,
                                                        CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new CreateLearningPathCommand(request, userId), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateLearningPath([FromQuery, Required] string learningPathId, 
        //[FromQuery] string UserId,
        [FromBody] UpdateLearningPathRequest request, CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new UpdateLearningPathCommand(learningPathId, userId, request), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }


    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteLearningPath([FromQuery, Required] string learningPathId,
                                                        //[FromQuery] string UserId,
                                                        CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new DeleteLearningPathCommand(learningPathId, userId), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }

}
