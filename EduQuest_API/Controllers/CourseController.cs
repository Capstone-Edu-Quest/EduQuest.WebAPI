using EduQuest_Application.DTO.Request;
using EduQuest_Application.UseCases.Courses.Cpmmands.CreateCourse;
using EduQuest_Application.UseCases.Courses.Queries.SearchCourse;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/Course")]
	public class CourseController : BaseController
	{
		private ISender _mediator;
		public CourseController(ISender mediator)
		{
			_mediator = mediator;

		}

		[HttpGet("searchCourse")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> SearchCourse([FromBody] SearchCourseRequest request, [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new SearchCourseQuery(pageNo, eachPage, request), cancellationToken);
			return Ok(result);
		}

		[HttpGet("byCourseId")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> SearchCourseById([FromBody] SearchCourseRequest request, [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new SearchCourseQuery(pageNo, eachPage, request), cancellationToken);
			return Ok(result);
		}

		[HttpPost("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new CreateCourseCommand(request), cancellationToken);
			return Ok(result);
		}
	}
}
