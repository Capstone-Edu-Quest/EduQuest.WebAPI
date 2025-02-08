using EduQuest_Application.DTO.Request.Courses;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Courses.Command.CreateCourse
{
    public class CreateCourseCommand : IRequest<APIResponse>
	{
        public CreateCourseRequest CourseRequest { get; set; }
		public string UserId { get; set; }

		public CreateCourseCommand(CreateCourseRequest courseRequest, string userId)
		{
			CourseRequest = courseRequest;
			UserId = userId;
		}
	}
}
