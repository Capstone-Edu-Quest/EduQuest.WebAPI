using AutoMapper;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.Lessons;
using EduQuest_Application.DTO.Response.Materials;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Query.GetLearnerDetailForInstructor
{
	public class GetLearnerDetailForInstructorQueryHandler : IRequestHandler<GetLearnerDetailForInstructorQuery, APIResponse>
	{
		private readonly ILessonRepository _lessonRepository;
		private readonly ICourseRepository _courseRepository;
		private readonly IMaterialRepository _materialRepository;
		private readonly ILessonMaterialRepository _lessonMaterialRepository;
		private readonly IQuizAttemptRepository _quizAttemptRepository;
		private readonly IAssignmentAttemptRepository _assignmentAttemptRepository;
		private readonly IMapper _mapper;

		public GetLearnerDetailForInstructorQueryHandler(ILessonRepository lessonRepository, 
			ICourseRepository courseRepository, 
			IMaterialRepository materialRepository, 
			ILessonMaterialRepository lessonMaterialRepository, 
			IQuizAttemptRepository quizAttemptRepository, 
			IAssignmentAttemptRepository assignmentAttemptRepository, 
			IMapper mapper)
		{
			_lessonRepository = lessonRepository;
			_courseRepository = courseRepository;
			_materialRepository = materialRepository;
			_lessonMaterialRepository = lessonMaterialRepository;
			_quizAttemptRepository = quizAttemptRepository;
			_assignmentAttemptRepository = assignmentAttemptRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(GetLearnerDetailForInstructorQuery request, CancellationToken cancellationToken)
		{
			//Lấy Course và các Lesson liên quan
			var course = await _courseRepository.GetById(request.CourseId);

			//Lấy các MaterialId liên quan đến Lesson
			var lessonIds = course.Lessons.Select(x => x.Id).ToList();

			//Lọc các QuizId và AssignmentId
			var materialIds = (await _lessonMaterialRepository.GetByListLessonId(lessonIds)).Select(x => x.MaterialId).Distinct().ToList();

			//Lọc các QuizId và AssignmentId
			var quizIds = (await _materialRepository.GetMaterialsByType(materialIds, GeneralEnums.TypeOfMaterial.Quiz.ToString())).Select(x => x.QuizId).ToList();
			var assignmentIds = (await _materialRepository.GetMaterialsByType(materialIds, GeneralEnums.TypeOfMaterial.Assignment.ToString())).Select(x => x.AssignmentId).ToList();


			// Lấy Attempt cho Quiz & Assignment
			var quizAttempts = await _quizAttemptRepository.GetQuizzesAttempts(quizIds, lessonIds, request.UserId);
			var assignmentAttempts = await _assignmentAttemptRepository.GetAssignmentAttempts(assignmentIds, lessonIds, request.UserId);
			
			// Tìm tất cả các lesson có attempt
			var distinctLessonIds = quizAttempts.Select(x => x.LessonId)
			.Union(assignmentAttempts.Select(x => x.LessonId))
			.Distinct()
			.ToList();

			var lessons = (await _lessonRepository.GetByIdsAsync(distinctLessonIds)).OrderBy(x => x.Index).ToList();
			
			var result = lessons.Select(lesson => new LessonBasicResponse
			{
				Id = lesson.Id,
				Name = lesson.Name,
				Quizzes = _mapper.Map<List<QuizAttemptsResponse>>(quizAttempts
				.Where(q => q.LessonId == lesson.Id)
				.OrderBy(q => q.CreatedAt))
				.Select(q =>
				{
					q.Author = null;
					return q;
				})
				.ToList(),

				Assignments = _mapper.Map<List<AssignmentAttemptResponseForInstructor>>(assignmentAttempts
				.Where(a => a.LessonId == lesson.Id))
				.Select(a =>
				{
					a.Author = null;
					return a;
				})
				.ToList()
			}).ToList();

			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, result, "name", $"Learner Detail for Course with ID {request.CourseId}");
		}
	}
}
