using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Courses.Query.GetLearnerOverviewForInstructor
{
	public class GetLearnerOverviewForInstructorQuery : IRequest<APIResponse>
	{
        public string CourseId { get; set; }

		public GetLearnerOverviewForInstructorQuery(string courseId)
		{
			CourseId = courseId;
		}
	}
}
