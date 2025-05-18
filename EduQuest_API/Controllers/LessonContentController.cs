using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.LessonContents.Command.UpdateLessonContent;
using EduQuest_Application.UseCases.LessonContents.Query.GetAllMyMaterial;
using EduQuest_Application.UseCases.LessonContents.Query.GetLessonContentById;
using EduQuest_Application.UseCases.Materials.Command.CreateLessonContent;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/lessonContent")]
	public class LessonContentController : BaseController
	{
		private ISender _mediator;
		public LessonContentController(ISender mediator)
		{
			_mediator = mediator;
		}

		[Authorize]
		[HttpGet("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetAllMaterial([FromQuery] SearchLessonContent info, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new GetAllMyMaterialQuery(userId, info), cancellationToken);
			return Ok(result);
		}

		[Authorize]
		[HttpPost("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateLessonContent([FromBody] List<CreateLessonContentRequest> request, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new CreateLeaningMaterialCommand(userId, request), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}

		[Authorize]
		[HttpPut("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateMaterial([FromBody] UpdateLessonContentRequest request, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new UpdateLessonContentCommand(userId, request), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}

		[Authorize]
		[HttpGet("byId")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetLessonContentById([FromQuery] int type, string lessonContentId, CancellationToken cancellationToken = default)
		{
			//string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new GetLessonContentByIdQuery(type, lessonContentId), cancellationToken);
			return Ok(result);
		}
	}
}
