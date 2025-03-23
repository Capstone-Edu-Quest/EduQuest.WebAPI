using EduQuest_Application.DTO.Request.Courses;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Command.UpdateCourse
{
	public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILessonRepository _stageRepository;
		private readonly IMaterialRepository _learningMaterialRepository;

		public UpdateCourseCommandHandler(ICourseRepository courseRepository, IUnitOfWork unitOfWork, ILessonRepository stageRepository, IMaterialRepository learningMaterialRepository)
		{
			_courseRepository = courseRepository;
			_unitOfWork = unitOfWork;
			_stageRepository = stageRepository;
			_learningMaterialRepository = learningMaterialRepository;
		}

		public async Task<APIResponse> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
		{
			var apiResponse = new APIResponse();
			var existingCourse = await _courseRepository.GetCourseById(request.CourseInfo.CourseId);
			if(existingCourse == null)
			{
				return apiResponse = GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound, $"Not Found {request.CourseInfo.CourseId}", "name", "Course");
				
			}
			existingCourse.Title = request.CourseInfo.Title;
			existingCourse.Description = request.CourseInfo.Description;
			existingCourse.PhotoUrl = request.CourseInfo.PhotoUrl;
			existingCourse.Requirement = request.CourseInfo.Requirement;
			existingCourse.Feature = request.CourseInfo.Feature;
			existingCourse.Price = request.CourseInfo.Price;

			var newStages = new List<Lesson>();
			if (request.CourseInfo.LessonCourse != null && request.CourseInfo.LessonCourse.Any())
			{
				
				await _stageRepository.DeleteStagesByCourseId(existingCourse.Id);

				for (int i = 0; i < request.CourseInfo.LessonCourse.Count; i++)
				{
					var stageRequest = request.CourseInfo.LessonCourse[i];
					var learningMaterials = await _learningMaterialRepository.GetMaterialsByIds(stageRequest.MaterialIds);
					var stage = new Lesson
					{
						Id = Guid.NewGuid().ToString(),
						Name = stageRequest.Name,
						Description = stageRequest.Description,
						CourseId = existingCourse.Id,
						Level = i + 1, 
						Materials = learningMaterials, // Gán Material
						TotalTime = learningMaterials?.Sum(m => m.Duration) ?? 0
					};

					newStages.Add(stage);
				}

				await _stageRepository.CreateRangeAsync(newStages);

				existingCourse.CourseStatistic.TotalTime = newStages.Sum(c => c.TotalTime);
				existingCourse.CourseStatistic.TotalLesson = newStages.Sum(stage => stage.Materials?.Count ?? 0);

				await _courseRepository.Update(existingCourse);
				await _unitOfWork.SaveChangesAsync();

			}
			return apiResponse = GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.UpdateSuccesfully, newStages, "name", "Course and Stages");

		}
	}
}
