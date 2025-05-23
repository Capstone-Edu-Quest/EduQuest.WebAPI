﻿using EduQuest_Application.DTO.Request.Courses;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Courses.Command.UpdateCourse
{
	public class UpdateCourseCommand : IRequest<APIResponse>
	{
        public string UserId { get; set; }
        public UpdateCourseRequest CourseInfo { get; set; }

		public UpdateCourseCommand(string userId, UpdateCourseRequest courseInfo)
		{
			UserId = userId;
			CourseInfo = courseInfo;
		}
	}
}
