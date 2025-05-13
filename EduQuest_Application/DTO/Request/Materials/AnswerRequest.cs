using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Materials
{
	public class AnswerRequest : IMapTo<Option>, IMapFrom<Option>
	{
		public string AnswerContent { get; set; }
		public bool IsCorrect { get; set; }
	}
}
