using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.FavoriteCourse.Command.AddFavoriteList;
using EduQuest_Application.UseCases.FavoriteCourse.Command.DeleteFavoriteList;
using EduQuest_Application.UseCases.FavoriteCourse.Query.GetUserFavoriteList;
using EduQuest_Application.UseCases.FavoriteCourse.Query.SearchFavoriteCourse;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;

namespace EduQuest_API.Controllers
{
	[Authorize(Roles = "Learner")]
	[Route(Constants.Http.API_VERSION + "/favoriteList")]
	public class FavoriteListController : BaseController
	{
		private ISender _mediator;
		public FavoriteListController(ISender mediator)
		{
			_mediator = mediator;

		}

		[HttpPut("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateFavCourse([FromBody] List<string> courseId, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new UpdateFavoriteListCommand(userId, courseId), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}

		//[HttpGet("search")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> SearchFavCourse([FromQuery] string? name, [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10, CancellationToken cancellationToken = default)
		//{
		//	string userId = User.GetUserIdFromToken().ToString();
		//	var result = await _mediator.Send(new SearchFavoriteCourseQuery(pageNo, eachPage, name, userId), cancellationToken);
		//	return Ok(result);
		//}

		[HttpDelete("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> DeleteFavCourse([FromQuery] string courseId, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new DeleteFavoriteListCommand(userId, courseId), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}
		[HttpGet]
		public async Task<IActionResult> GetUserFavoriteList(//[FromQuery] string userId,
			[FromQuery, AllowNull] string? title,
			[FromQuery, AllowNull] string? description,
			[FromQuery, AllowNull] string? requirement, 
			[FromQuery, AllowNull] string? feature,
			[FromQuery, AllowNull] decimal? price,
            [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10,
            CancellationToken token = default)
		{
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new GetUserFavoriteListQuery(userId, title, description, price, 
				requirement, feature, pageNo, eachPage), token);
            if ((result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK))
            {
                return BadRequest(result);
            }
            PagedList<FavoriteCourseResponse> list = (PagedList<FavoriteCourseResponse>)result.Payload!;
            Response.Headers.Add("X-Total-Element", list.TotalItems.ToString());
            Response.Headers.Add("X-Total-Page", list.TotalPages.ToString());
            Response.Headers.Add("X-Current-Page", list.CurrentPage.ToString());
            return Ok(result);
        }
	}
}
