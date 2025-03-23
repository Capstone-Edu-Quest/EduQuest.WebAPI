using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;

namespace EduQuest_Application.UseCases.Materials.Command.CreateLeaningMaterial
{
    public class CreateLeaningMaterialCommand : IRequest<APIResponse>
	{
        public string UserId { get; set; }
        public List<CreateLearningMaterialRequest> Material { get; set; }

		public CreateLeaningMaterialCommand(string userId, List<CreateLearningMaterialRequest> material)
		{
			UserId = userId;
			Material = material;
		}
	}
}
