using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.LessonContents.Command.CreateAssignment;
using EduQuest_Application.UseCases.LessonContents.Command.CreateQuiz;
using EduQuest_Application.UseCases.LessonContents.Query.GetQuizById;
using EduQuest_Application.UseCases.Materials.Command.CreateMaterial;
using EduQuest_Application.UseCases.Materials.Command.UpdateMaterial;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

		//[Authorize]
		[HttpGet("byId")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetQuizById([FromQuery] string quizId, CancellationToken cancellationToken = default)
		{
			//string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new GetQuizByIdQuery(quizId), cancellationToken);
			return Ok(result);
		}

		[Authorize]
		[HttpPost("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizRequest request, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new CreateQuizCommand(userId, request), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}

		[Authorize]
		[HttpPut("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateQuiz([FromBody] CreateQuizRequest request, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new CreateQuizCommand(userId, request), cancellationToken);

			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}
	}
}
