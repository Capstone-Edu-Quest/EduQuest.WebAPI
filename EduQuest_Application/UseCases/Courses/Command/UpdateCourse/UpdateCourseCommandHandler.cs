using AutoMapper;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using Stripe;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Command.UpdateCourse
{
	public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ILessonRepository _lessonRepository;
		private readonly ILessonMaterialRepository _lessonMaterialRepository;
		private readonly IMaterialRepository _materialRepository;
		private readonly IUserMetaRepository _userMetaRepository;
		private readonly ITagRepository _tagRepository;

		public UpdateCourseCommandHandler(ICourseRepository courseRepository, IUnitOfWork unitOfWork, IMapper mapper, ILessonRepository lessonRepository, ILessonMaterialRepository lessonMaterialRepository, IMaterialRepository materialRepository, IUserMetaRepository userMetaRepository, ITagRepository tagRepository)
		{
			_courseRepository = courseRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_lessonRepository = lessonRepository;
			_lessonMaterialRepository = lessonMaterialRepository;
			_materialRepository = materialRepository;
			_userMetaRepository = userMetaRepository;
			_tagRepository = tagRepository;
		}

		public async Task<APIResponse> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
		{
			var apiResponse = new APIResponse();
			var existingCourse = await _courseRepository.GetCourseById(request.CourseInfo.CourseId);
			if (existingCourse == null)
			{
				return apiResponse = GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound, $"Not Found {request.CourseInfo.CourseId}", "name", "Course");
			}
			var courseResponse = new CourseResponseForUpdate();
			var listTag = await _tagRepository.GetByIdsAsync(request.CourseInfo.TagIds);
			if (existingCourse.Status.ToLower() != GeneralEnums.StatusCourse.Public.ToString().ToLower())
			{
				existingCourse.Title = request.CourseInfo.Title;
				existingCourse.Description = request.CourseInfo.Description;
				existingCourse.PhotoUrl = request.CourseInfo.PhotoUrl;
				existingCourse.Requirement = ContentHelper.JoinStrings(request.CourseInfo.RequirementList, '.');
				existingCourse.Price = request.CourseInfo.Price;
				existingCourse.Tags.Clear();
				existingCourse.Tags = listTag;

				var newLessons = new List<Lesson>();
				if (request.CourseInfo.LessonCourse != null && request.CourseInfo.LessonCourse.Any())
				{
					await _lessonRepository.DeleteLessonByCourseId(existingCourse.Id);
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
							Index = i,
							TotalTime = materials?.Sum(m => m.Duration) ?? 0
						};
						
						var lessonMaterials = materials.Select(m => new LessonMaterial
						{
							LessonId = lesson.Id,
							MaterialId = m.Id,
							Index = materials.IndexOf(m),
							CreatedAt = DateTime.Now.ToUniversalTime(),
							UpdatedAt = DateTime.Now.ToUniversalTime(),
						}).ToList();

						lesson.LessonMaterials = lessonMaterials;

						newLessons.Add(lesson);
					}

					// Lưu bài học vào cơ sở dữ liệu
					await _lessonRepository.CreateRangeAsync(newLessons);


					
				}
				existingCourse.CourseStatistic.TotalTime = newLessons.Sum(c => c.TotalTime);
				existingCourse.CourseStatistic.TotalLesson = newLessons.Count();
				await _courseRepository.Update(existingCourse);

				var result = await _unitOfWork.SaveChangesAsync();
				if (result > 0)
				{
					var course = await _courseRepository.GetById(request.CourseInfo.CourseId);
					courseResponse = _mapper.Map<CourseResponseForUpdate>(course);
					courseResponse.Lessons = newLessons;
					courseResponse.RequirementList = ContentHelper.SplitString(course.Requirement, '.');
				}
				return apiResponse = GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.UpdateSuccesfully, courseResponse, "name", "Course and Lesson");
			}
			else
			{
				var updateMaterialIds = new List<string>();

				foreach (var lesson in request.CourseInfo.LessonCourse)
				{
					updateMaterialIds.AddRange(lesson.MaterialIds);  // Giữ nguyên tất cả các materialIds
				}

				var oldMaterialIds = new List<string>();

				var lessons = existingCourse.Lessons;
				foreach(var lesson in lessons)
				{
					var materialIds = await _lessonMaterialRepository.GetListMaterialIdByLessonId(lesson.Id);
					oldMaterialIds.AddRange(materialIds);
				}

				var percentage = CompareMaterialIds(updateMaterialIds, oldMaterialIds);
				if(percentage >= 0.3)
				{
					return apiResponse = GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, $"Update Failed {request.CourseInfo.CourseId}", "name", "Course");
				} else
				{
					existingCourse.Title = request.CourseInfo.Title;
					existingCourse.Description = request.CourseInfo.Description;
					existingCourse.PhotoUrl = request.CourseInfo.PhotoUrl;
					existingCourse.Requirement = ContentHelper.JoinStrings(request.CourseInfo.RequirementList, '.');
					existingCourse.Price = request.CourseInfo.Price;
					existingCourse.Tags.Clear();
					existingCourse.Tags = listTag;

					var newLessons = new List<Lesson>();
					if (request.CourseInfo.LessonCourse != null && request.CourseInfo.LessonCourse.Any())
					{
						await _lessonRepository.DeleteLessonByCourseId(existingCourse.Id);
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
								TotalTime = (int?)materials?.Sum(m => m.Duration) ?? 0
							};
							TotalLesson += materials.Count();
							var lessonMaterials = materials.Select(m => new LessonMaterial
							{
								LessonId = lesson.Id,
								MaterialId = m.Id,
								Index = materials.IndexOf(m),
								CreatedAt = DateTime.Now.ToUniversalTime(),
								UpdatedAt = DateTime.Now.ToUniversalTime(),
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

					var result = await _unitOfWork.SaveChangesAsync();
					if (result > 0)
					{
						var course = await _courseRepository.GetById(request.CourseInfo.CourseId);
						courseResponse = _mapper.Map<CourseResponseForUpdate>(course);
						courseResponse.Lessons = newLessons;
						courseResponse.RequirementList = ContentHelper.SplitString(course.Requirement, '.');
					}

				}

				return apiResponse = GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.CreateSuccesfully, courseResponse, "name", $"New Version of Course ID {existingCourse.Id}");
			}
		}

		private double CompareMaterialIds(List<string> updateMaterialIds, List<string> oldMaterialIds)
		{
			int count = 0;

			int maxLength = Math.Max(updateMaterialIds.Count, oldMaterialIds.Count);

			for (int i = 0; i < maxLength; i++)
			{
				string updateMaterial = i < updateMaterialIds.Count ? updateMaterialIds[i] : null;
				string oldMaterial = i < oldMaterialIds.Count ? oldMaterialIds[i] : null;

				if (updateMaterial != oldMaterial)
				{
					count++;
				}
			}

			count += Math.Abs(updateMaterialIds.Count - oldMaterialIds.Count);
			var result = count / oldMaterialIds.Count();
			return result;
		}


	}
}
