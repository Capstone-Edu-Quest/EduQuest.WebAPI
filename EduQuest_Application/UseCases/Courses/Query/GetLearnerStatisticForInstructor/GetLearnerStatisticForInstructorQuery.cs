using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Courses.Query.GetLearnerStatisticForInstructor
{
	public class GetLearnerStatisticForInstructorQuery : IRequest<APIResponse>
	{
        public string CourseId { get; set; }

		public GetLearnerStatisticForInstructorQuery(string courseId)
		{
			CourseId = courseId;
		}
	}
}
