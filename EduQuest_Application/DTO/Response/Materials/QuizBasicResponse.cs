using AutoMapper;
using EduQuest_Application.DTO.Response.Certificates;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Materials
{
	public class QuizBasicResponse : IMapFrom<QuizAttempt>, IMapTo<QuizAttempt>
	{
        public string QuizId { get; set; }
        public double? Percentage { get; set; }

	}
}
