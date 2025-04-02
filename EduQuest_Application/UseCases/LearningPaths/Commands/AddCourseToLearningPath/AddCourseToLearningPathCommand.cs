using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.LearningPaths.Commands.AddCourseToLearningPath
{
	public class AddCourseToLearningPathCommand : IRequest<APIResponse>
	{
        public string UserId { get; set; }
        public List<string> ListCourseId { get; set; }
        public string LearningPathId { get; set; }

		public AddCourseToLearningPathCommand(string userId, List<string> listCourseId, string learningPathId)
		{
			UserId = userId;
			ListCourseId = listCourseId;
			LearningPathId = learningPathId;
		}
	}
}
