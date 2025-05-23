﻿using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Lessons
{
	public class UpdateLessonRequest : IMapFrom<Lesson>, IMapTo<Lesson>
	{
        public string Id { get; set; }
        public string? Name { get; set; }
		public string? Description { get; set; }

		public List<string> MaterialIds { get; set; }
	}
}
