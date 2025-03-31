using EduQuest_Application.DTO.Response.Materials;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Lessons
{
	public class LessonCourseResponse : IMapFrom<Lesson>, IMapTo<Lesson>
    {
        public string Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public int? TotalTime { get; set; }
        public List<MaterialInLessonResponse> Materials { get; set; }

    }
}
