using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.LessonContents.Query.GetAllMyMaterial
{
	public class GetAllMyMaterialQuery : IRequest<APIResponse>
	{
		public string UserId { get; set; }
		public SearchLessonContent Info { get; set; }

		public GetAllMyMaterialQuery(string userId, SearchLessonContent info)
		{
			UserId = userId;
			Info = info;
		}
	}
}
