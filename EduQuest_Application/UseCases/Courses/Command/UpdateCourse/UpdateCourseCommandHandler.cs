using AutoMapper;
using EduQuest_Application.DTO.Request.Lessons;
using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.Courses.Command.UpdateCourse
{
	public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ILessonRepository _lessonRepository;
		private readonly ILessonContentRepository _lessonMaterialRepository;
		private readonly IMaterialRepository _materialRepository;
		private readonly IUserMetaRepository _userMetaRepository;
		private readonly ITagRepository _tagRepository;
		private readonly ILearnerRepository _learnerRepository;
		private readonly IQuizRepository _quizRepository;
		private readonly IAssignmentRepository _assignmentRepository;

		public UpdateCourseCommandHandler(ICourseRepository courseRepository, 
			IUnitOfWork unitOfWork, IMapper mapper, 
			ILessonRepository lessonRepository, 
			ILessonContentRepository lessonMaterialRepository, 
			IMaterialRepository materialRepository, 
			IUserMetaRepository userMetaRepository, 
			ITagRepository tagRepository, 
			ILearnerRepository learnerRepository, 
			IQuizRepository quizRepository, 
			IAssignmentRepository assignmentRepository)
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
			_quizRepository = quizRepository;
			_assignmentRepository = assignmentRepository;
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
						var processLessonContent = await GetOrderedContentsAndTotalTime(lessonRequest.ContentIds);

						var lesson = new Lesson
						{
							Id = Guid.NewGuid().ToString(),
							Name = lessonRequest.Name,
							Description = lessonRequest.Description,
							CourseId = existingCourse.Id,
							Index = i,
							TotalTime = processLessonContent.TotalTime
						};

						var lessonContents = processLessonContent.OrderedContents.Select((x, index) => new LessonContent
						{
							LessonId = lesson.Id,
							MaterialId = (x.Request.Type == (int)TypeOfMaterial.Video || x.Request.Type == (int)TypeOfMaterial.Document) ? x.Request.Id : null,
							QuizId = x.Request.Type == (int)TypeOfMaterial.Quiz ? x.Request.Id : null,
							AssignmentId = x.Request.Type == (int)TypeOfMaterial.Assignment ? x.Request.Id : null,
							Index = index,
							CreatedAt = DateTime.Now.ToUniversalTime(),
							UpdatedAt = DateTime.Now.ToUniversalTime(),
						}).ToList();

						lesson.LessonContents = lessonContents;

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
					updateMaterialIds.AddRange(lesson.ContentIds.Select(x => x.Id));  // Giữ nguyên tất cả các materialIds
				}

				var oldExistMaterialIds = new List<string>();

				var lessons = existingCourse.Lessons.OrderBy(l => l.Index);
				foreach(var lesson in lessons)
				{
					var materialIds = await _lessonMaterialRepository.GetListContentIdByLessonId(lesson.Id);
					oldExistMaterialIds.AddRange(materialIds);
				}

				var percentage = CompareContentIds(updateMaterialIds, oldExistMaterialIds);
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
								var processLessonContent = await GetOrderedContentsAndTotalTime(lessonRequest.ContentIds);

								var lesson = new Lesson
								{
									CourseId = request.CourseInfo.CourseId,
									Name = lessonRequest.Name,
									Description = lessonRequest.Description,
									Index = i,
									TotalTime = processLessonContent.TotalTime, 
									CreatedAt = DateTime.Now.ToUniversalTime(),
									UpdatedAt = DateTime.Now.ToUniversalTime(),
								};

								var lessonContents = processLessonContent.OrderedContents.Select((x, index) => new LessonContent
								{
									LessonId = lesson.Id,
									MaterialId = (x.Request.Type == (int)TypeOfMaterial.Video || x.Request.Type == (int)TypeOfMaterial.Document) ? x.Request.Id : null,
									QuizId = x.Request.Type == (int)TypeOfMaterial.Quiz ? x.Request.Id : null,
									AssignmentId = x.Request.Type == (int)TypeOfMaterial.Assignment ? x.Request.Id : null,
									Index = index,
									CreatedAt = DateTime.Now.ToUniversalTime(),
									UpdatedAt = DateTime.Now.ToUniversalTime(),
								}).ToList();

								lesson.LessonContents = lessonContents;
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

							// Lấy tất cả các contentId đang có (MaterialId, QuizId, AssignmentId)
							var existingContentIds = lastLesson.LessonContents
								.Select(lm => lm.MaterialId ?? lm.QuizId ?? lm.AssignmentId)
								.ToList();

							// Lấy contentId mới chưa tồn tại
							var newContentRequests = lessonRequest.ContentIds
								.Where(x => !existingContentIds.Contains(x.Id))
								.ToList();

							if (newContentRequests.Any())
							{
								var processLessonContent = await GetOrderedContentsAndTotalTime(newContentRequests);

								var lessonContents = processLessonContent.OrderedContents.Select((x, index) => new LessonContent
								{
									LessonId = lastLesson.Id,
									MaterialId = (x.Request.Type == (int)TypeOfMaterial.Video || x.Request.Type == (int)TypeOfMaterial.Document) ? x.Request.Id : null,
									QuizId = x.Request.Type == (int)TypeOfMaterial.Quiz ? x.Request.Id : null,
									AssignmentId = x.Request.Type == (int)TypeOfMaterial.Assignment ? x.Request.Id : null,
									Index = index,
									CreatedAt = DateTime.Now.ToUniversalTime(),
									UpdatedAt = DateTime.Now.ToUniversalTime(),
								}).ToList();

								await _lessonMaterialRepository.CreateRangeAsync(lessonContents);

								// Update lại lesson
								lastLesson.TotalTime += processLessonContent.TotalTime; // ví dụ: tính lại tổng thời lượng
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
					var totalLessonContent = await _lessonMaterialRepository.GetTotalContent(request.CourseInfo.CourseId);
					foreach(var learner in listLearnerNotComplete)
					{
						learner.ProgressPercentage = Math.Round((await _lessonRepository.CalculateContentProgressBeforeCurrentAsync(learner.CurrentLessonId, learner.CurrentContentIndex, totalLessonContent)) * 100, 2);
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

				return apiResponse = GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.UpdateSuccesfully, courseResponse, "name", $"Course ID {existingCourse.Id}");
			}
		}

		private double CompareContentIds(List<string> updateMaterialIds, List<string> oldMaterialIds)
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

		private async Task<(List<(LessonContentRequest Request, object Entity)> OrderedContents, double TotalTime)> GetOrderedContentsAndTotalTime(List<LessonContentRequest> ContentIds)
		{
			var materialIds = ContentIds
				.Where(x => x.Type == (int)TypeOfMaterial.Video || x.Type == (int)TypeOfMaterial.Document)
				.Select(x => x.Id)
				.ToList();

			var quizIds = ContentIds
				.Where(x => x.Type == (int)TypeOfMaterial.Quiz)
				.Select(x => x.Id)
				.ToList();

			var assignmentIds = ContentIds
				.Where(x => x.Type == (int)TypeOfMaterial.Assignment)
				.Select(x => x.Id)
				.ToList();

			var materialEntities = await _materialRepository.GetByIdsAsync(materialIds);
			var quizEntities = await _quizRepository.GetByIdsAsync(quizIds);
			var assignmentEntities = await _assignmentRepository.GetByIdsAsync(assignmentIds);

			var orderedContents = new List<(LessonContentRequest Request, object Entity)>();

			foreach (var content in ContentIds)
			{
				switch (content.Type)
				{
					case (int)TypeOfMaterial.Video:
					case (int)TypeOfMaterial.Document:
						var material = materialEntities.FirstOrDefault(m => m.Id == content.Id);
						if (material != null)
							orderedContents.Add((content, (object)material));
						break;
					case (int)TypeOfMaterial.Quiz:
						var quiz = quizEntities.FirstOrDefault(q => q.Id == content.Id);
						if (quiz != null)
							orderedContents.Add((content, (object)quiz));
						break;
					case (int)TypeOfMaterial.Assignment:
						var assignment = assignmentEntities.FirstOrDefault(a => a.Id == content.Id);
						if (assignment != null)
							orderedContents.Add((content, (object)assignment));
						break;
				}
			}

			var totalTime = orderedContents.Sum(x =>
			{
				switch (x.Request.Type)
				{
					case (int)TypeOfMaterial.Video:
					case (int)TypeOfMaterial.Document:
						return ((Material)x.Entity).Duration;
					case (int)TypeOfMaterial.Quiz:
						return ((Quiz)x.Entity).TimeLimit;
					case (int)TypeOfMaterial.Assignment:
						return ((Assignment)x.Entity).TimeLimit;
					default:
						return 0;
				}
			});

			return (orderedContents, (double)totalTime);

		}



	}
}
