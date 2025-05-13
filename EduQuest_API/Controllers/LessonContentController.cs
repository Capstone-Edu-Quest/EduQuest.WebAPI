using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.LessonContents.Query.GetAllMyMaterial;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
		public async Task<IActionResult> GetAllMaterial(CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new GetAllMyMaterialQuery(userId), cancellationToken);
			return Ok(result);
		}


	}
}
