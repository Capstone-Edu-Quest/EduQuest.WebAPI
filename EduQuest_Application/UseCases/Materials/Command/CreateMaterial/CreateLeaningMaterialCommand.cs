using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;

namespace EduQuest_Application.UseCases.Materials.Command.CreateMaterial
{
    public class CreateLeaningMaterialCommand : IRequest<APIResponse>
	{
        public string UserId { get; set; }
        public List<CreateMaterialRequest> Material { get; set; }

		public CreateLeaningMaterialCommand(string userId, List<CreateMaterialRequest> material)
		{
			UserId = userId;
			Material = material;
		}
	}
}
