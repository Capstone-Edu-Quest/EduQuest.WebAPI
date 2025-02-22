using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Courses
{
    public class CreateCourseRequest : IMapFrom<Course>, IMapTo<Course>
    {

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Requirement { get; set; }
        public string? Feature { get; set; }
		public decimal? Price { get; set; }
		public bool? IsRequired { get; set; }
        //public List<string> Tags { get; set; }
        //public List<StageCourseRequest>? StageCourse { get; set; }
    }
}
