using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Materials
{
	public class UpdateQuizRequest : IMapFrom<Quiz>, IMapTo<Quiz>
	{
		//public string Id { get; set; }
		//public string? Title { get; set; }
		//public string? Description { get; set; }
		public int TimeLimit { get; set; }
		public decimal PassingPercentage { get; set; }
		public List<QuestionRequest> Questions { get; set; }
	}
}
