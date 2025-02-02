using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response
{
	public class AuthorCourseResponse : IMapFrom<User>, IMapTo<User>
	{
        public string Id { get; set; }
        public string Username { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
		public int? TotalCourseCreated { get; set; }
		public int? TotalLearner { get; set; }
		public double? Rating { get; set; }
		public int? TotalReview { get; set; }
	}
}
