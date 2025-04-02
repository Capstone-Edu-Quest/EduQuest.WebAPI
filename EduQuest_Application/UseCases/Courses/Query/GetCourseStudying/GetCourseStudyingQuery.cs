using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Courses.Query.GetCourseStudying
{
	public class GetCourseStudyingQuery : IRequest<APIResponse>
	{
        public string UserId { get; set; }

		public GetCourseStudyingQuery(string userId)
		{
			UserId = userId;
		}
	}
}
