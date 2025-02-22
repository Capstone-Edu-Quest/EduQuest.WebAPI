using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Materials
{
	public class QuestionRequest
	{
		public string QuestionTitle { get; set; }
		public bool MultipleAnswers { get; set; }
        public List<AnswerRequest> AnswerRequest { get; set; }
    }
}
