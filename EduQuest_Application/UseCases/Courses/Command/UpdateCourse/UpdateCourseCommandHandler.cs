using AutoMapper;
using EduQuest_Application.DTO.Request.Courses;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using Stripe;
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
		private readonly IMapper _mapper;
		private readonly ILessonRepository _lessonRepository;
		private readonly IMaterialRepository _materialRepository;
		private readonly IUserMetaRepository _userMetaRepository;

		

		public async Task<APIResponse> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
		{
			var apiResponse = new APIResponse();
			var existingCourse = await _courseRepository.GetCourseById(request.CourseInfo.CourseId);
			if(existingCourse == null)
			{
				return apiResponse = GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound, $"Not Found {request.CourseInfo.CourseId}", "name", "Course");
				
			}
			if(!existingCourse.CourseLearners.Any() && existingCourse.Status.ToLower() == GeneralEnums.StatusCourse.Draft.ToString().ToLower())
			{
				existingCourse.Title = request.CourseInfo.Title;
				existingCourse.Description = request.CourseInfo.Description;
				existingCourse.PhotoUrl = request.CourseInfo.PhotoUrl;
				existingCourse.Requirement = ContentHelper.JoinStrings(request.CourseInfo.RequirementList, '.');
				existingCourse.Price = request.CourseInfo.Price;

				var newLessons = new List<Lesson>();
				if (request.CourseInfo.LessonCourse != null && request.CourseInfo.LessonCourse.Any())
				{

					await _lessonRepository.DeleteStagesByCourseId(existingCourse.Id);
					int TotalLesson = 0;
					for (int i = 0; i < request.CourseInfo.LessonCourse.Count; i++)
					{
						var lessonRequest = request.CourseInfo.LessonCourse[i];
						var materials = await _materialRepository.GetMaterialsByIds(lessonRequest.MaterialIds);
						materials = materials.OrderBy(m => lessonRequest.MaterialIds.IndexOf(m.Id)).ToList();

						var lesson = new Lesson
						{
							Id = Guid.NewGuid().ToString(),
							Name = lessonRequest.Name,
							Description = lessonRequest.Description,
							CourseId = existingCourse.Id,
							Index = i + 1,
							TotalTime = materials?.Sum(m => m.Duration) ?? 0
						};
						TotalLesson += materials.Count();
						var lessonMaterials = materials.Select(m => new LessonMaterial
						{
							LessonId = lesson.Id,
							MaterialId = m.Id,
							Index = materials.IndexOf(m), 
						}).ToList();

						lesson.LessonMaterials = lessonMaterials;

						newLessons.Add(lesson);
					}

					// Lưu bài học vào cơ sở dữ liệu
					await _lessonRepository.CreateRangeAsync(newLessons);


					existingCourse.CourseStatistic.TotalTime = newLessons.Sum(c => c.TotalTime);
					existingCourse.CourseStatistic.TotalLesson = TotalLesson;

					await _courseRepository.Update(existingCourse);
				}
			
				await _unitOfWork.SaveChangesAsync();
				return apiResponse = GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.UpdateSuccesfully, newLessons, "name", "Course and Lesson");
			} else
			{
				var newCourse = _mapper.Map<Course>(request.CourseInfo);
				newCourse.Id = Guid.NewGuid().ToString();
				newCourse.Requirement = ContentHelper.JoinStrings(request.CourseInfo.RequirementList, '.');
				newCourse.CreatedBy = request.UserId;
				newCourse.Status = GeneralEnums.StatusCourse.Draft.ToString();
				newCourse.Version = existingCourse.Version + 1;
				newCourse.OriginalCourseId = existingCourse.Id;
				newCourse.CourseStatistic = new CourseStatistic
				{
					Id = Guid.NewGuid().ToString(),
					CourseId = newCourse.Id,
					TotalLearner = 0,
					TotalLesson = 0,
					TotalReview = 0,
					Rating = 0,
					TotalTime = 0
				};
				await _courseRepository.Add(newCourse);
				var userMeta = await _userMetaRepository.GetByUserId(request.UserId);
				userMeta.TotalCourseCreated++;
				await _userMetaRepository.Update(userMeta);
				await _unitOfWork.SaveChangesAsync();

				var newLessons = new List<Lesson>();
				if (request.CourseInfo.LessonCourse != null && request.CourseInfo.LessonCourse.Any())
				{

					int TotalLesson = 0;
					for (int i = 0; i < request.CourseInfo.LessonCourse.Count; i++)
					{
						var lessonRequest = request.CourseInfo.LessonCourse[i];
						var materials = await _materialRepository.GetMaterialsByIds(lessonRequest.MaterialIds);
						materials = materials.OrderBy(m => lessonRequest.MaterialIds.IndexOf(m.Id)).ToList();

						var lesson = new Lesson
						{
							Id = Guid.NewGuid().ToString(),
							Name = lessonRequest.Name,
							Description = lessonRequest.Description,
							CourseId = existingCourse.Id,
							Index = i + 1,
							TotalTime = materials?.Sum(m => m.Duration) ?? 0
						};
						TotalLesson += materials.Count();
						var lessonMaterials = materials.Select(m => new LessonMaterial
						{
							LessonId = lesson.Id,
							MaterialId = m.Id,
							Index = materials.IndexOf(m),
						}).ToList();

						lesson.LessonMaterials = lessonMaterials;

						newLessons.Add(lesson);
					}

					await _lessonRepository.CreateRangeAsync(newLessons);


					existingCourse.CourseStatistic.TotalTime = newLessons.Sum(c => c.TotalTime);
					existingCourse.CourseStatistic.TotalLesson = TotalLesson;

					await _courseRepository.Update(newCourse);
				}
				var result = await _unitOfWork.SaveChangesAsync();
				return apiResponse = GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.CreateSuccesfully, newCourse, "name", $"New Version of Course ID {existingCourse.Id}");
			}
		}
	}
}
