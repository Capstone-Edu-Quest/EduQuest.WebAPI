using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Courses.Query.GetCourseStatisticForInstructor
{
	public class GetCourseStatisticForInstructorQuery : IRequest<APIResponse>
	{
        public string UserId { get; set; }

		public GetCourseStatisticForInstructorQuery(string userId)
		{
			UserId = userId;
		}
	}
}
