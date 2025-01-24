using EduQuest_Application.DTO.Request;
using EduQuest_Application.UseCases.Achievements.Commands.CreateAchievement;
using EduQuest_Application.UseCases.Achievements.Commands.UpdateAchievement;
using EduQuest_Application.UseCases.FavoriteCourse.Commands.AddFavoriteList;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/achievement")]
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
		public async Task<IActionResult> AddAchievement([FromBody] CreateAchievementRequest achievement, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new CreateAchievementCommand(achievement), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}

		[HttpPut("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateAchievement([FromBody] UpdateAchievementRequest achievement, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new UpdateAchievementCommand(achievement), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}
	}
}
