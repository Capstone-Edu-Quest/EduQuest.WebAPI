using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.Materials;

namespace EduQuest_Application.DTO.Response.Lessons
{
	public class LessonBasicResponse
	{
        public string Id { get; set; }
        public string Name { get; set; }
        public List<QuizAttemptsResponse>? Quizzes { get; set; }
		public List<AssignmentAttemptResponseForInstructor>? Assignments { get; set; }
	}
}
