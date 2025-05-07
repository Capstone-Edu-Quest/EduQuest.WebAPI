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
		private readonly ILearnerRepository _learnerRepository;

		public UpdateCourseCommandHandler(ICourseRepository courseRepository, 
			IUnitOfWork unitOfWork, 
			IMapper mapper, 
			ILessonRepository lessonRepository, 
			ILessonMaterialRepository lessonMaterialRepository, 
			IMaterialRepository materialRepository, 
			IUserMetaRepository userMetaRepository, 
			ITagRepository tagRepository, 
			ILearnerRepository learnerRepository)
		{
			_courseRepository = courseRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_lessonRepository = lessonRepository;
			_lessonMaterialRepository = lessonMaterialRepository;
			_materialRepository = materialRepository;
			_userMetaRepository = userMetaRepository;
			_tagRepository = tagRepository;
			_learnerRepository = learnerRepository;
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
			existingCourse.Tags.Clear();
			existingCourse.Tags = listTag;

			if (existingCourse.Status.ToLower() != GeneralEnums.StatusCourse.Public.ToString().ToLower())
			{
				existingCourse.Title = request.CourseInfo.Title;
				existingCourse.Description = request.CourseInfo.Description;
				existingCourse.PhotoUrl = request.CourseInfo.PhotoUrl;
				existingCourse.Requirement = ContentHelper.JoinStrings(request.CourseInfo.RequirementList, '.');
				existingCourse.Price = request.CourseInfo.Price;
				

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

				var oldExistMaterialIds = new List<string>();

				var lessons = existingCourse.Lessons.OrderBy(l => l.Index);
				foreach(var lesson in lessons)
				{
					var materialIds = await _lessonMaterialRepository.GetListMaterialIdByLessonId(lesson.Id);
					oldExistMaterialIds.AddRange(materialIds);
				}

				var percentage = CompareMaterialIds(updateMaterialIds, oldExistMaterialIds);
				if(percentage > 0.3)
				{
					return apiResponse = GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, MessageError.Over30Percentage, MessageError.Over30Percentage, "name", $"Course ID {request.CourseInfo.CourseId}");
				} else
				{
					existingCourse.Title = request.CourseInfo.Title;
					existingCourse.Description = request.CourseInfo.Description;
					existingCourse.PhotoUrl = request.CourseInfo.PhotoUrl;
					existingCourse.Requirement = ContentHelper.JoinStrings(request.CourseInfo.RequirementList, '.');
					existingCourse.Price = request.CourseInfo.Price;

					var listExistingLesson = (await _lessonRepository.GetByCourseId(request.CourseInfo.CourseId))
					.OrderBy(l => l.Index)
					.ToList();

					var newLessons = new List<Lesson>();

					if (request.CourseInfo.LessonCourse != null && request.CourseInfo.LessonCourse.Any())
					{
						var lessonRequests = request.CourseInfo.LessonCourse;

						// Check trường hợp thêm Lesson mới vào cuối
						if (lessonRequests.Count > listExistingLesson.Count)
						{
							// Thêm các lesson mới vào cuối
							for (int i = listExistingLesson.Count; i < lessonRequests.Count; i++)
							{
								var lessonRequest = lessonRequests[i];
								var materials = await _materialRepository.GetMaterialsByIds(lessonRequest.MaterialIds);
								materials = materials.OrderBy(m => lessonRequest.MaterialIds.IndexOf(m.Id)).ToList();

								var lesson = new Lesson
								{
									CourseId = request.CourseInfo.CourseId,
									Name = lessonRequest.Name,
									Description = lessonRequest.Description,
									Index = i,
									TotalTime = materials.Sum(m => m.Duration), // ví dụ tính thời gian
									CreatedAt = DateTime.Now.ToUniversalTime(),
									UpdatedAt = DateTime.Now.ToUniversalTime(),
								};

								var lessonMaterials = materials.Select((m, index) => new LessonMaterial
								{
									Lesson = lesson,
									MaterialId = m.Id,
									Index = index,
									CreatedAt = DateTime.Now.ToUniversalTime(),
									UpdatedAt = DateTime.Now.ToUniversalTime()
								}).ToList();

								lesson.LessonMaterials = lessonMaterials;
								newLessons.Add(lesson);
							}

							await _lessonRepository.CreateRangeAsync(newLessons);
						}
						// Nếu không thêm lesson mới, chỉ được phép thêm material vào lesson cuối cùng
						else if (lessonRequests.Count == listExistingLesson.Count)
						{
							var lastLessonIndex = listExistingLesson.Count - 1;
							var lastLesson = listExistingLesson[lastLessonIndex];
							var lessonRequest = lessonRequests[lastLessonIndex];

							// Chỉ xử lý nếu có thêm material mới
							var oldMaterialIds = lastLesson.LessonMaterials.Select(lm => lm.MaterialId).ToList();
							var newMaterialIds = lessonRequest.MaterialIds.Except(oldMaterialIds).ToList();

							if (newMaterialIds.Any())
							{
								var newMaterials = await _materialRepository.GetMaterialsByIds(newMaterialIds);
								var sortedNewMaterials = newMaterials.OrderBy(m => lessonRequest.MaterialIds.IndexOf(m.Id)).ToList();

								var newLessonMaterials = sortedNewMaterials.Select((m, index) => new LessonMaterial
								{
									LessonId = lastLesson.Id,
									MaterialId = m.Id,
									Index = oldMaterialIds.Count + index,
									CreatedAt = DateTime.Now.ToUniversalTime(),
									UpdatedAt = DateTime.Now.ToUniversalTime()
								}).ToList();

								await _lessonMaterialRepository.CreateRangeAsync(newLessonMaterials);

								// Update lại lesson
								lastLesson.TotalTime += sortedNewMaterials.Sum(m => m.Duration); // ví dụ: tính lại tổng thời lượng
								await _lessonRepository.Update(lastLesson);
							}
						}
					}
					await _unitOfWork.SaveChangesAsync();

					//Update course statistic
					var finalLessons = await _lessonRepository.GetByCourseId(request.CourseInfo.CourseId);
					existingCourse.CourseStatistic.TotalTime = finalLessons.Sum(c => c.TotalTime);
					existingCourse.CourseStatistic.TotalLesson = finalLessons.Count();
					await _courseRepository.Update(existingCourse);

					//Update Learner progress
					var listLearnerNotComplete = (await _learnerRepository.GetListLearnerOfCourse(request.CourseInfo.CourseId)).Where(l => l.ProgressPercentage < 100);
					var totalMaterial = await _lessonMaterialRepository.GetTotalMaterial(request.CourseInfo.CourseId);
					foreach(var learner in listLearnerNotComplete)
					{
						learner.ProgressPercentage = Math.Round((await _lessonRepository.CalculateMaterialProgressBeforeCurrentAsync(learner.CurrentLessonId, learner.CurrentMaterialId, totalMaterial)) * 100, 2);
					}
					await _learnerRepository.UpdateRangeAsync(listLearnerNotComplete);

					var result = await _unitOfWork.SaveChangesAsync();
					if (result > 0)
					{
						var course = await _courseRepository.GetById(request.CourseInfo.CourseId);
						courseResponse = _mapper.Map<CourseResponseForUpdate>(course);
						courseResponse.Lessons = finalLessons;
						courseResponse.RequirementList = ContentHelper.SplitString(course.Requirement, '.');
					}

				}

				return apiResponse = GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.CreateSuccesfully, courseResponse, "name", $"New Version of Course ID {existingCourse.Id}");
			}
		}

		private double CompareMaterialIds(List<string> updateMaterialIds, List<string> oldMaterialIds)
		{
			int count = 0;

			//int maxLength = Math.Max(updateMaterialIds.Count, oldMaterialIds.Count);

			//for (int i = 0; i < maxLength; i++)
			//{
			//	string updateMaterial = i < updateMaterialIds.Count ? updateMaterialIds[i] : null;
			//	string oldMaterial = i < oldMaterialIds.Count ? oldMaterialIds[i] : null;

			//	if (updateMaterial != oldMaterial)
			//	{
			//		count++;
			//	}
			//}

			count += Math.Abs(updateMaterialIds.Count - oldMaterialIds.Count);
			double result = (double)count / (double)oldMaterialIds.Count();
			return result;
		}


	}
}
