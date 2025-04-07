using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Materials.Command.CreateMaterial;
using EduQuest_Application.UseCases.Materials.Command.DeleteMaterial;
using EduQuest_Application.UseCases.Materials.Command.UpdateMaterial;
using EduQuest_Application.UseCases.Materials.Command.UploadVideo;
using EduQuest_Application.UseCases.Materials.Query.GetAllMyMaterial;
using EduQuest_Application.UseCases.Materials.Query.GetDetailMaterial;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/material")]
	public class MaterialController : BaseController
	{
		private ISender _mediator;
		public MaterialController(ISender mediator)
		{
			_mediator = mediator;
		}

		[Authorize]
		[HttpPost("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateMaterial([FromBody] List<CreateLearningMaterialRequest> request, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new CreateLeaningMaterialCommand(userId, request), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
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

		[Authorize]
		[HttpDelete("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> DeleteMaterial([FromQuery] string materialId, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new DeleteMaterialCommand(materialId, userId), cancellationToken);
			return Ok(result);
		}

		[Authorize]
		[HttpPut("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateMaterial([FromBody] UpdateLearningMaterialRequest request, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new UpdateMaterialCommand(userId, request), cancellationToken);
			return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
		}

        [Authorize]
        [HttpGet("detail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDetailMaterial(CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new GetDetailMaterialQuery(userId), cancellationToken);
            return Ok(result);
        }

		[Authorize]
		[HttpPost("uploadVideo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadChunk([FromForm] UploadVideoRequest request)
        {
            //string userId = User.GetUserIdFromToken().ToString();
            using var memoryStream = new MemoryStream();
            await request.Chunk.CopyToAsync(memoryStream);
            byte[] chunkData = memoryStream.ToArray();

            var command = new UploadVideoCommand
            {
                FileId = request.FileId,
                ChunkIndex = request.ChunkIndex,
                TotalChunks = request.TotalChunks,
                ChunkData = chunkData

            };

            var response = await _mediator.Send(command);
            return Ok(response);
        }

    }
}
