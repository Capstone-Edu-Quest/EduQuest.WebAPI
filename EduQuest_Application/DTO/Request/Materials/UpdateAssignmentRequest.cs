using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Materials
{
	public class UpdateAssignmentRequest : IMapFrom<Assignment>, IMapTo<Assignment>
	{
		//public string Id { get; set; }
		//public string? Title { get; set; }
		//public string? Description { get; set; }
		public double? TimeLimit { get; set; }
		public string? Question { get; set; }
		public string? AnswerLanguage { get; set; }
		public string? ExpectedAnswer { get; set; }
	}
}
