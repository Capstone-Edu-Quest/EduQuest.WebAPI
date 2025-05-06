using EduQuest_Application.DTO.Request.Revenue;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Revenue.Query.GetChartRevenue;
using EduQuest_Application.UseCases.Revenue.Query.GetCourseRevenue;
using EduQuest_Application.UseCases.Revenue.Query.GetDetailRevenueTransaction;
using EduQuest_Application.UseCases.Revenue.Query.GetRevenueForAdmin;
using EduQuest_Application.UseCases.Revenue.Query.GetRevenueReportForIntructor;
using EduQuest_Application.UseCases.Revenue.Query.GetRevenueTransaction;
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

		[HttpGet("myRevenueTransaction")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetMyRevenueTransaction([FromQuery] DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new GetRevenueTransactionQuery(userId, dateFrom, dateTo), cancellationToken);
			return Ok(result);
		}

		[HttpGet("detailRevenueTransaction")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetDetailRevenueTransaction([FromQuery] string transactionDetailId, CancellationToken cancellationToken = default)
		{
			//string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new GetDetailRevenueTransactionQuery(transactionDetailId), cancellationToken);
			return Ok(result);
		}

		[HttpGet("adminRevenueTransaction")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetAdminRevenueTransaction([FromQuery] RevenueTransactionForAdmin request, CancellationToken cancellationToken = default)
		{
			//string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new GetRevenueForAdminQuery(request), cancellationToken);
			return Ok(result);
		}
	}
}
