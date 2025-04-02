using EduQuest_Application.DTO.Request.Quests;
using EduQuest_Application.DTO.Response.Quests;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Achievements.Commands.UpdateAchievement;
using EduQuest_Application.UseCases.Quests.Command.CreateQuest;
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

        //[Authorize(Roles ="Staff, Admin")]
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
            //[FromQuery] string questId,
            //[FromQuery] string userId,
            CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new UpdateQuestCommand(userId, achievement), cancellationToken);
            return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
        }

        [Authorize(Roles = "Staff, Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllSystemQuests(//[FromQuery] string userId,
            [FromQuery, AllowNull] string? title,
            [FromQuery, AllowNull] int? questType,
            [FromQuery, AllowNull] int? type,
            [FromQuery, AllowNull] int? questValue,
            [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10,
             CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new GetAllSystemQuestsQuery(title, questType, type, questValue, userId, pageNo, eachPage), cancellationToken);
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

        [Authorize(Roles = "Learner")]
        [HttpGet("user")]
        public async Task<IActionResult> GetAllUserQuests(//[FromQuery] string userId,
            [FromQuery, AllowNull] string? title,
            [FromQuery, AllowNull] int? questType,
            [FromQuery, AllowNull] int? type,
            [FromQuery, AllowNull] int? pointToComplete,
            [FromQuery, AllowNull] DateTime? startDate,
            [FromQuery, AllowNull] DateTime? dueDate,
            [FromQuery, AllowNull] bool? isComplete = false,          
            [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10,
        CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new GetAllUserQuestsQuery(title, questType,
                type, pointToComplete, startDate, dueDate, isComplete, userId, pageNo, eachPage), cancellationToken);
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

        [Authorize(Roles = "Learner")]
        [HttpPost("user")]
        public async Task<IActionResult> ClaimReward([FromQuery] string userQuestId, 
            //[FromQuery] string userId,
            CancellationToken token = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            throw new NotImplementedException();
        }
    }
}
