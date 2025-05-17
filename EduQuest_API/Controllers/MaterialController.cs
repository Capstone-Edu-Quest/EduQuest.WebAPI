using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Materials.Command.DeleteMaterial;
using EduQuest_Application.UseCases.Materials.Command.UploadImage;
using EduQuest_Application.UseCases.Materials.Command.UploadVideo;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
		[HttpDelete("")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> DeleteMaterial([FromQuery] string materialId, CancellationToken cancellationToken = default)
		{
			string userId = User.GetUserIdFromToken().ToString();
			var result = await _mediator.Send(new DeleteMaterialCommand(materialId, userId), cancellationToken);
			return Ok(result);
		}

		

       

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


        [HttpPost("uploadImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> uploadImage([FromForm] UploadImageCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

    }
}
