using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Materials.Command.UpdateMaterial
{
	public class UpdateMaterialCommand : IRequest<APIResponse>
	{
		public string UserId { get; set; }
		public UpdateMaterialRequest Material { get; set; }

		public UpdateMaterialCommand(string userId, UpdateMaterialRequest material)
		{
			UserId = userId;
			Material = material;
		}
	}
}
