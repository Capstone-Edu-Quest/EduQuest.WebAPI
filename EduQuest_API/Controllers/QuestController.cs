using EduQuest_Application.DTO.Request.Quests;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Achievements.Commands.CreateAchievement;
using EduQuest_Application.UseCases.Achievements.Commands.UpdateAchievement;
using EduQuest_Application.UseCases.FavoriteCourse.Command.AddFavoriteList;
using EduQuest_Application.UseCases.Quests.Command.CreateQuest;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

		//[Authorize]
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

        //[Authorize]
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
			throw new NotImplementedException();
		}

		//[HttpGet]

	}
}
