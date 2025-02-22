using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Materials
{
	public class QuizRequest
	{
		public int TimeLimit { get; set; }
		public decimal PassingPercentage { get; set; }
        public List<QuestionRequest> QuestionRequest { get; set; }
    }
}
