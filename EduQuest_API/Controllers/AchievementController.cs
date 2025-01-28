using EduQuest_Application.DTO.Request;
using EduQuest_Application.UseCases.Achievements.Commands.CreateAchievement;
using EduQuest_Application.UseCases.Achievements.Commands.UpdateAchievement;
using EduQuest_Application.UseCases.FavoriteCourse.Commands.AddFavoriteList;
using EduQuest_Application.UseCases.Quests.Commands.CreateQuest;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/quest")]
	public class AchievementController : BaseController
	{
		private ISender _mediator;
		public AchievementController(ISender mediator)
		{
			_mediator = mediator;

		}

		[HttpPost("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddQuest([FromBody] CreateQuestRequest achievement, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new CreateQuestCommand(achievement), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}

		[HttpPut("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateQuest([FromBody] UpdateQuestRequest achievement, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new UpdateQuestCommand(achievement), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}
	}
}
