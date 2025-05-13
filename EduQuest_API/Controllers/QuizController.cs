using EduQuest_Application.UseCases.LessonContents.Query.GetQuixById;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/quiz")]
	public class QuizController : ControllerBase
	{
		private ISender _mediator;
		public QuizController(ISender mediator)
		{
			_mediator = mediator;
		}

		[Authorize]
		[HttpGet("byId")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetQuizById([FromQuery] string quizId, CancellationToken cancellationToken = default)
		{
			//string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new GetQuizByIdQuery(quizId), cancellationToken);
			return Ok(result);
		}
	}
}
