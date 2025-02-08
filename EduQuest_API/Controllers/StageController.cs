using EduQuest_Application.DTO.Request.Courses;
using EduQuest_Application.DTO.Request.Stages;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Courses.Command.CreateCourse;
using EduQuest_Application.UseCases.Courses.Queries.GetCourseById;
using EduQuest_Application.UseCases.Stages.Command.CreateStage;
using EduQuest_Application.UseCases.Stages.Command.UpdateStage;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/stage")]
	public class StageController : BaseController
	{
		private ISender _mediator;
		public StageController(ISender mediator)
		{
			_mediator = mediator;

		}

		[Authorize]
		[HttpPost("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateStage([FromQuery] string courseId, [FromBody] List<CreateStageRequest> request, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new CreateStageCommand(courseId, request), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}

		[Authorize]
		[HttpPatch("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateStage([FromBody] List<UpdateStageRequest> request, CancellationToken cancellationToken = default)
		{			
			var result = await _mediator.Send(new UpdateStageCommand(request), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}
	}
}
