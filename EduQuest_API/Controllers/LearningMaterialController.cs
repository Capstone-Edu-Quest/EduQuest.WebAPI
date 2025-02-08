using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Application.DTO.Request.Stages;
using EduQuest_Application.UseCases.LearningMaterials.Command.CreateLeaningMaterial;
using EduQuest_Application.UseCases.LearningMaterials.Command.UpdateLeaningMaterial;
using EduQuest_Application.UseCases.Stages.Command.CreateStage;
using EduQuest_Application.UseCases.Stages.Command.UpdateStage;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduQuest_API.Controllers
{
    [Route(Constants.Http.API_VERSION + "/learningmaterial")]
	public class LearningMaterialController : BaseController
	{
		private ISender _mediator;
		public LearningMaterialController(ISender mediator)
		{
			_mediator = mediator;
		}

		//[Authorize]
		[HttpPost("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateMaterial([FromBody] List<CreateLearningMaterialRequest> request, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new CreateLeaningMaterialCommand(request), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}

		//[Authorize]
		[HttpPatch("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateMaterial([FromBody] List<UpdateLearningMaterialRequest> request, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new UpdateLeaningMaterialCommand(request), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}
	}
}
