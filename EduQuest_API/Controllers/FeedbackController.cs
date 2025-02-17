using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using EduQuest_Application.DTO.Request.Feedbacks;
using System.Diagnostics.CodeAnalysis;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Application.DTO.Response.Feedbacks;
using EduQuest_Application.UseCases.Feedbacks.Queries.GetCourseFeedbackQuery;
using EduQuest_Application.UseCases.Feedbacks.Commands.CreateFeedback;
using EduQuest_Application.Helper;
using Microsoft.AspNetCore.Authorization;
using EduQuest_Application.UseCases.Feedbacks.Commands.UpdateFeedback;
using EduQuest_Application.UseCases.Feedbacks.Commands.DeteleFeedback;

namespace EduQuest_API.Controllers;

[Route(Constants.Http.API_VERSION + "/feedback")]
[ApiController]
public class FeedbackController : ControllerBase
{
    private ISender _mediator;
    public FeedbackController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetCourseFeedback([FromQuery, AllowNull] int? rating, [FromQuery, AllowNull] string? comment, 
        [FromQuery, Required] string courseId,
        [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10,
        CancellationToken token = default)
    {
        //string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new GetCourseFeedbackQuery(courseId, pageNo, eachPage, rating, comment), token);
        if ((result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK))
        {
            return BadRequest(result);
        }
        PagedList<FeedbackResponse> list = (PagedList<FeedbackResponse>)result.Payload!;
        Response.Headers.Add("X-Total-Element", list.TotalItems.ToString());
        Response.Headers.Add("X-Total-Page", list.TotalPages.ToString());
        Response.Headers.Add("X-Current-Page", list.CurrentPage.ToString());
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateFeedback([FromBody, Required] CreateFeedbackRequest feeback,
        //[FromQuery] string UserId,
        CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new CreateFeedbackCommand(feeback, userId), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateFeedback([FromBody, Required] UpdateFeedbackRequest feedback, 
        [FromQuery, Required] string feedbackId,
        //[FromQuery] string UserId,
        CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new UpdateFeedbackCommand(userId, feedbackId, feedback), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteFeedback([FromQuery, Required] string feedbackId,
        //[FromQuery] string UserId,
        CancellationToken token = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new DeteleFeedbackCommand(userId, feedbackId), token);
        return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }

}
