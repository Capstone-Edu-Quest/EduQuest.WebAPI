﻿using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Request.Lessons
{
	public class CreateLessonRequest : IMapFrom<Lesson>, IMapTo<Lesson>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

		public List<LessonContentRequest>? ContentIds { get; set; }
		
    }
}
