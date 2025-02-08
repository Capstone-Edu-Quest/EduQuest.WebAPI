using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.LearningMaterials.Command.UpdateLeaningMaterial
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
