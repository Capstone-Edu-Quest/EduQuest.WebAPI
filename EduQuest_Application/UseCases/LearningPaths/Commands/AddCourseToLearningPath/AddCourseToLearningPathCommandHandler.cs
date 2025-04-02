using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.LearningPaths.Commands.AddCourseToLearningPath
{
	public class AddCourseToLearningPathCommandHandler : IRequestHandler<AddCourseToLearningPathCommand, APIResponse>
	{
		//private readonly ILearningPathRepository _learningPathRepository;
		//public async Task<APIResponse> Handle(AddCourseToLearningPathCommand request, CancellationToken cancellationToken)
		//{
		//	var listLP = await _learningPathRepository.GetMySpecificLearningPath(request.UserId, request.LearningPathId);
		//	var listCourseId = listLP.LearningPathCourses.Select(x => x.CourseId).Distinct();
		//}
		public Task<APIResponse> Handle(AddCourseToLearningPathCommand request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
