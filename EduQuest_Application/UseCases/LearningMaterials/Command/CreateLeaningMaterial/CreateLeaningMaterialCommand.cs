using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;

namespace EduQuest_Application.UseCases.LearningMaterials.Command.CreateLeaningMaterial
{
    public class CreateLeaningMaterialCommand : IRequest<APIResponse>
	{
        public List<CreateLearningMaterialRequest> Material { get; set; }

		public CreateLeaningMaterialCommand(List<CreateLearningMaterialRequest> material)
		{
			Material = material;
		}
	}
}
