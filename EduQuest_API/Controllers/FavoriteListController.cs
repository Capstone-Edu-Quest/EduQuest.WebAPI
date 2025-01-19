using EduQuest_Application.DTO.Request;
using EduQuest_Application.UseCases.Courses.Queries.GetCourseById;
using EduQuest_Application.UseCases.Courses.Queries.SearchCourse;
using EduQuest_Application.UseCases.FavoriteCourse.Commands.AddFavoriteList;
using EduQuest_Application.UseCases.FavoriteCourse.Commands.DeleteFavoriteList;
using EduQuest_Application.UseCases.FavoriteCourse.Queries.SearchFavoriteCourse;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/favoriteList")]
	public class FavoriteListController : BaseController
	{
		private ISender _mediator;
		public FavoriteListController(ISender mediator)
		{
			_mediator = mediator;

		}

		[HttpGet("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddFavCourse([FromQuery] string courseId, string userId, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new AddFavoriteListCommand(userId, courseId), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}

		[HttpGet("search")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> SearchFavCourse([FromQuery] string? name, string userId, [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new SearchFavoriteCourseQuery(pageNo, eachPage, name, userId), cancellationToken);
			return Ok(result);
		}

		[HttpDelete("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> DeleteFavCourse([FromQuery] string courseId, string userId, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new DeleteFavoriteListCommand(userId, courseId), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}
	}
}
