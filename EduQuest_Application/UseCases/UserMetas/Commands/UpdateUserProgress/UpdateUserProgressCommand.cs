using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.UserMetas.Commands.UpdateUserProgress
{
	public class UpdateUserProgressCommand : IRequest<APIResponse>
	{
        public string UserId { get; set; }
        public string CourseId { get; set; }
        public string MaterialId { get; set; }
        public int? Time { get; set; }

		public UpdateUserProgressCommand(string userId, string courseId, string materialId, int? time)
		{
			UserId = userId;
			CourseId = courseId;
			MaterialId = materialId;
			Time = time;
		}
	}
}
