using EduQuest_Application.DTO.Request.Quests;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.DTO.Response.Quests;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Achievements.Commands.CreateAchievement;
using EduQuest_Application.UseCases.Achievements.Commands.UpdateAchievement;
using EduQuest_Application.UseCases.FavoriteCourse.Command.AddFavoriteList;
using EduQuest_Application.UseCases.Quests.Command.CreateQuest;
using EduQuest_Application.UseCases.Quests.Queries;
using EduQuest_Application.UseCases.Quests.Queries.GetAllSystemQuests;
using EduQuest_Application.UseCases.Quests.Queries.GetAllUserQuests;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace EduQuest_API.Controllers
{
    [Route(Constants.Http.API_VERSION + "/quest")]
    public class QuestController : BaseController
    {
        private ISender _mediator;
        public QuestController(ISender mediator)
        {
            _mediator = mediator;

        }

        [Authorize(Roles ="Staff, Admin")]
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddQuest([FromBody] CreateQuestRequest quest,
            [FromQuery] string userId,
            CancellationToken cancellationToken = default)
        {
            //string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new CreateQuestCommand(userId, quest), cancellationToken);
            return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
        }

        [Authorize(Roles = "Staff, Admin")]
        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateQuest([FromBody] UpdateQuestRequest achievement,
            [FromQuery] string questId,
            [FromQuery] string userId,
            CancellationToken cancellationToken = default)
        {
            //string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new UpdateQuestCommand(userId, questId, achievement), cancellationToken);
            return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
        }

        [Authorize(Roles = "Staff, Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllSystemQuests([FromQuery, AllowNull] string? title,
            [FromQuery, AllowNull] string? description,
            [FromQuery, AllowNull] int pointToComplete,
            [FromQuery, AllowNull] int type,
            [FromQuery, AllowNull] int timeToComplete,
            [FromQuery] string userId,
            [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10,
             CancellationToken cancellationToken = default)
        {
            //string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new GetAllSystemQuestsQuery(title, description,
                pointToComplete, type, timeToComplete, userId, pageNo, eachPage), cancellationToken);
            if ((result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK))
            {
                return BadRequest(result);
            }
            PagedList<QuestResponse> list = (PagedList<QuestResponse>)result.Payload!;
            Response.Headers.Add("X-Total-Element", list.TotalItems.ToString());
            Response.Headers.Add("X-Total-Page", list.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", list.CurrentPage.ToString());
            return Ok(result);
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetAllUserQuests([FromQuery, AllowNull] string? title,
            [FromQuery, AllowNull] string? description,
            [FromQuery, AllowNull] int pointToComplete,
            [FromQuery, AllowNull] int type,
            [FromQuery, AllowNull] DateTime? startDate,
            [FromQuery, AllowNull] DateTime? dueDate,
            [FromQuery] string userId,
            [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10,
        CancellationToken cancellationToken = default)
        {
            //string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new GetAllUserQuestsQuery(title, description,
                pointToComplete, type, startDate, dueDate, userId, pageNo, eachPage), cancellationToken);
            if ((result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK))
            {
                return BadRequest(result);
            }
            PagedList<UserQuestResponse> list = (PagedList<UserQuestResponse>)result.Payload!;
            Response.Headers.Add("X-Total-Element", list.TotalItems.ToString());
            Response.Headers.Add("X-Total-Page", list.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", list.CurrentPage.ToString());
            return Ok(result);
        }
    }
}
