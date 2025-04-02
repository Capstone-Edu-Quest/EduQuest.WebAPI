using EduQuest_Application.DTO.Request.Courses;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Courses.Command.CreateCourse;
using EduQuest_Application.UseCases.Courses.Command.UpdateCourse;
using EduQuest_Application.UseCases.Courses.Queries;
using EduQuest_Application.UseCases.Courses.Queries.GetCourseById;
using EduQuest_Application.UseCases.Courses.Queries.SearchCourse;
using EduQuest_Application.UseCases.Courses.Query.GetCourseByStatus;
using EduQuest_Application.UseCases.Courses.Query.GetCourseDetailForIntructor;
using EduQuest_Application.UseCases.Courses.Query.GetCourseStudying;
using EduQuest_Application.UseCases.Expert.Commands.ApproveCourse;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/course")]
	public class CourseController : BaseController
	{
		private ISender _mediator;
		public CourseController(ISender mediator)
		{
			_mediator = mediator;

		}

        [HttpPut("approve")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApprovePendingCourse([FromBody] ApproveCourseCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        //[HttpPut("assign")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> ApprovePendingCourse([FromBody] AssignToCommand command, CancellationToken cancellationToken = default)
        //{
        //    var result = await _mediator.Send(command, cancellationToken);
        //    return Ok(result);
        //}

        [HttpGet("status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCourseByStatus([FromQuery] GetCourseByStatusQuery query, CancellationToken cancellationToken = default)
        {
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("searchCourse")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> SearchCourse([FromQuery] SearchCourseRequest request, [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new SearchCourseQuery(pageNo, eachPage, userId, request), cancellationToken);
			return Ok(result);
		}

		[HttpGet("courseDetailForInstructor")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CourseDetailResponseForIntructor([FromQuery] string courseId, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new GetCourseDetailForIntructorQuery(userId, courseId), cancellationToken);
			return Ok(result);
		}

		//[HttpGet("recommendedCourse")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> GetRecommnededCourse([FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10, CancellationToken cancellationToken = default)
		//{
		//	string userId = User.GetUserIdFromToken().ToString();
		//	var result = await _mediator.Send(new GetRecommendedCourseQuery(userId, pageNo, eachPage), cancellationToken);
		//	return Ok(result);
		//}

		[HttpGet("byCourseId")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> SearchCourseById([FromQuery] string courseId, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new GetCourseByIdQuery(userId, courseId), cancellationToken);
			return Ok(result);
		}

		[HttpGet("studying")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetCourseStudying(CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new GetCourseStudyingQuery(userId), cancellationToken);
			return Ok(result);
		}

		[Authorize]
		[HttpGet("createdByMe")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetCourseByUserId([FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new GetCourseCreatedByMeQuery(userId,pageNo, eachPage), cancellationToken);
			return Ok(result);
		}

		[Authorize]
		[HttpPost("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new CreateCourseCommand(request, userId), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}

		[Authorize]
		[HttpPut("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateCourse([FromBody] UpdateCourseRequest request, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new UpdateCourseCommand(userId, request), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}
	}
}
