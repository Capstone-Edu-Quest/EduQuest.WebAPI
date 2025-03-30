using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Materials.Command.UpdateLeaningMaterial
{
	public class UpdateLeaningMaterialCommand : IRequest<APIResponse>
	{
		public string UserId { get; set; }
		public UpdateLearningMaterialRequest Material { get; set; }

		public UpdateLeaningMaterialCommand(string userId, UpdateLearningMaterialRequest material)
		{
			UserId = userId;
			Material = material;
		}
	}
}
