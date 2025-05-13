using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
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
	}
}
