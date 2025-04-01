using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Courses.Query.GetCourseDetailForIntructor
{
	public class GetCourseDetailForIntructorQuery : IRequest<APIResponse>
	{
        public string UserId { get; set; }
        public string CourseId { get; set; }

		public GetCourseDetailForIntructorQuery(string userId, string courseId)
		{
			UserId = userId;
			CourseId = courseId;
		}
	}
}
