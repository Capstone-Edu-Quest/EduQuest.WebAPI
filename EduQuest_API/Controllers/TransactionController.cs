using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Revenue.Query.GetRevenueReportForIntructor;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/transaction")]
	public class TransactionController : BaseController
	{
		private ISender _mediator;

		public TransactionController(ISender mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("transactionDetail")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetTransactionDetail([FromQuery] string transactionId, CancellationToken cancellationToken = default)
		{
			var userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new GetRevenueReportForIntructorQuery(transactionId), cancellationToken);
			return Ok(result);
		}
	}
}
