using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Materials.Command.UpdateLeaningMaterial
{
	public class UpdateLeaningMaterialCommand : IRequest<APIResponse>
	{
		public List<UpdateLearningMaterialRequest> Material { get; set; }

		public UpdateLeaningMaterialCommand(List<UpdateLearningMaterialRequest> material)
		{
			Material = material;
		}
	}
}
