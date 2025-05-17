using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/assignment")]
	public class AssignmentController : ControllerBase
	{
		private ISender _mediator;
		public AssignmentController(ISender mediator)
		{
			_mediator = mediator;
		}

		//[HttpGet("assignmentById")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> GetAssignmentById([FromQuery] string assignmentId, CancellationToken cancellationToken = default)
		//{
		//	//string userId = User.GetUserIdFromToken().ToString();
		//	var result = await _mediator.Send(new GetAssignmentByIdQuery(assignmentId), cancellationToken);
		//	return Ok(result);
		//}

		//[Authorize]
		//[HttpPost("")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> CreateAssignment([FromBody] CreateAssignmentRequest request, CancellationToken cancellationToken = default)
		//{
		//	string userId = User.GetUserIdFromToken().ToString();
		//	var result = await _mediator.Send(new CreateAssignmentCommand(userId, request), cancellationToken);
		//	return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		//}

		//[Authorize]
		//[HttpPut("")]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//public async Task<IActionResult> UpdateAssignment([FromBody] UpdateAssignmentRequest request, CancellationToken cancellationToken = default)
		//{
		//	string userId = User.GetUserIdFromToken().ToString();
		//	var result = await _mediator.Send(new UpdateAssignmentCommand(userId, request), cancellationToken);

		//	return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		//}
	}
}
