using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Revenue.Query.GetChartRevenue;
using EduQuest_Application.UseCases.Revenue.Query.GetCourseRevenue;
using EduQuest_Application.UseCases.Revenue.Query.GetRevenueReportForIntructor;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/revenue")]
	public class RevenueController : BaseController
	{
		private ISender _mediator;

		public RevenueController(ISender mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("revenueReport")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetRenenueReport(CancellationToken cancellationToken = default)
		{
			var userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new GetRevenueReportForIntructorQuery(userId), cancellationToken);
			return Ok(result);
		}

		[HttpGet("courseRevenue")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetCourseRevenue(CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new GetCourseRevenueQuery(userId), cancellationToken);
			return Ok(result);
		}

		[HttpGet("chartRevenue")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetChartRevenue(CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new GetChartRevenueQuery(userId), cancellationToken);
			return Ok(result);
		}
	}
}
