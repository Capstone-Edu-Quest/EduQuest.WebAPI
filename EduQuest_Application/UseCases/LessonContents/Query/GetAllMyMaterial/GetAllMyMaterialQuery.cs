using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.LessonContents.Query.GetAllMyMaterial
{
	public class GetAllMyMaterialQuery : IRequest<APIResponse>
	{
        public string UserId { get; set; }

		public GetAllMyMaterialQuery(string userId)
		{
			UserId = userId;
		}
	}
}
