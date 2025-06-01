using AutoMapper;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.Lessons;
using EduQuest_Application.DTO.Response.Materials;
using EduQuest_Application.DTO.Response.Tags;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.Courses.Queries.GetCourseById
{
	public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly ILessonRepository _lessonRepository;
		private readonly ILessonContentRepository _lessonContentRepository;
		private readonly IMaterialRepository _materialRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly IUserMetaRepository _userStatisticRepository;

		public GetCourseByIdQueryHandler(ICourseRepository courseRepository, ILessonRepository lessonRepository, ILessonContentRepository lessonContentRepository, IMaterialRepository materialRepository, IUserRepository userRepository, IMapper mapper, IUserMetaRepository userStatisticRepository)
		{
			_courseRepository = courseRepository;
			_lessonRepository = lessonRepository;
			_lessonContentRepository = lessonContentRepository;
			_materialRepository = materialRepository;
			_userRepository = userRepository;
			_mapper = mapper;
			_userStatisticRepository = userStatisticRepository;
		}

		public async Task<APIResponse> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
		{
			var apiResponse = new APIResponse();
			var course = await _courseRepository.GetCourseById(request.CourseId);
			var courseWithLearner = await _courseRepository.GetCourseLearnerByCourseId(request.CourseId);
			var courseLearner = new CourseLearner();
			if (courseWithLearner != null && courseWithLearner.CourseLearners != null)
			{
				courseLearner = courseWithLearner.CourseLearners!.FirstOrDefault(x => x.UserId == request.UserId);
			}
			
			Lesson currentLesson = new Lesson(); 
			int currentMaterialIndex = 0;

			//Mapping course
			var courseResponse = _mapper.Map<CourseDetailResponse>(course);
			courseResponse.RequirementList = ContentHelper.SplitString(course.Requirement, '.');
			courseResponse.TotalLearner = course.CourseStatistic.TotalLearner;
			courseResponse.TotalReview = course.CourseStatistic.TotalReview;
			courseResponse.Rating = course.CourseStatistic.Rating;
			courseResponse.TotalTime = course.CourseStatistic.TotalTime;
			courseResponse.LastUpdated = course.UpdatedAt;
			

			if (courseLearner != null)
			{
				courseResponse.Progress = courseLearner!.ProgressPercentage;
				//Get Current Lesson
				if (courseLearner.CurrentLessonId != null)
				{
					currentLesson = await _lessonRepository.GetById(courseLearner.CurrentLessonId);
				}
				if (courseLearner.CurrentLessonId != null && courseLearner.CurrentContentIndex != null)
				{
					currentMaterialIndex = await _lessonContentRepository.GetCurrentContentIndex(courseLearner.CurrentLessonId, courseLearner.CurrentContentIndex);
				}
                courseResponse.CertificateId = course.Certificates.Where(c => c.UserId == request.UserId).FirstOrDefault() != null ?
                    course.Certificates.Where(c => c.UserId == request.UserId).FirstOrDefault().Id : null;

            } 

			courseResponse.Author = course.User! != null ? new AuthorCourseResponse
			{
				Id = course.User.Id,
				Username = course.User.Username ?? string.Empty,
				Headline = course.User.Headline ?? string.Empty,
				Description = course.User.Description ?? string.Empty
			} : null;

			var userSta = await _userStatisticRepository.GetByUserId(course.User!.Id);
			if (userSta != null)
			{
				courseResponse.Author!.TotalCourseCreated = userSta.TotalCourseCreated;
				courseResponse.Author.TotalReview = userSta.TotalReview;
				courseResponse.Author.TotalLearner = userSta.TotalLearner;
			}

			
			var lessonResponses = new List<LessonCourseResponse>();

			foreach (var lesson in course.Lessons.OrderBy(x => x.Index)!)
			{
				//var lessonInCourse = await _lessonRepository.GetByLessonIdAsync(lesson.Id);

				var contents = new List<ContentInLessonResponse>();

				var lessonContents = await _lessonContentRepository.GetContentsByLessonIdAsync(lesson.Id);

				// Nếu có OriginalMaterialId, thì lấy thêm thông tin của Material gốc
				//if (material.OriginalMaterialId != null)
				//{
				//	var originalMaterial = await _materialRepository.GetById(material.OriginalMaterialId);

				//	if (originalMaterial != null)
				//	{
				//		var originalMaterialResponse = new MaterialInLessonResponse
				//		{
				//			Id = originalMaterial.Id,
				//			Type = originalMaterial.Type,
				//			Duration = originalMaterial.Duration,
				//			Title = originalMaterial.Title,
				//			Description = originalMaterial.Description,
				//			Version = originalMaterial.Version,
				//			Status = GeneralEnums.StatusMaterial.Locked.ToString(),
				//		};

				//		materials.Add(originalMaterialResponse);
				//	}
				//}


				foreach (var content in lessonContents)
				{
					var materialResponse = new ContentInLessonResponse();

					// Xác định đúng entity đang dùng (chỉ 1 Id có giá trị)
					if (content.MaterialId != null)
					{
						materialResponse.Id = content.MaterialId;
						materialResponse.Type = content.Material.Type; // Hoặc lấy từ entity nếu có
						materialResponse.Duration = content.Material?.Duration;
						materialResponse.Title = content.Material?.Title;
						materialResponse.Description = content.Material?.Description;
					}
					else if (content.QuizId != null)
					{
						materialResponse.Id = content.QuizId;
						materialResponse.Type = TypeOfMaterial.Quiz.ToString();
						materialResponse.Duration = content.Quiz?.TimeLimit;
						materialResponse.Title = content.Quiz?.Title;
						materialResponse.Description = content.Quiz?.Description;
					}
					else if (content.AssignmentId != null)
					{
						materialResponse.Id = content.AssignmentId;
						materialResponse.Type = TypeOfMaterial.Assignment.ToString();
						materialResponse.Duration = content.Assignment?.TimeLimit;
						materialResponse.Title = content.Assignment?.Title;
						materialResponse.Description = content.Assignment?.Description;
					}

					// Xử lý Status theo tiến trình học
					if (courseLearner == null)
					{
						materialResponse.Status = GeneralEnums.StatusMaterial.Locked.ToString();
					}
					else if (courseLearner.ProgressPercentage < 100)
					{
						if ((currentLesson.Index == lesson.Index && content.Index > currentMaterialIndex) || currentLesson.Index < lesson.Index)
						{
							materialResponse.Status = GeneralEnums.StatusMaterial.Locked.ToString();
						}
						else if (currentLesson.Index == lesson.Index && content.Index == currentMaterialIndex)
						{
							materialResponse.Status = GeneralEnums.StatusMaterial.Current.ToString();
						}
						else
						{
							materialResponse.Status = GeneralEnums.StatusMaterial.Done.ToString();
						}
					}
					else
					{
						materialResponse.Status = GeneralEnums.StatusMaterial.Done.ToString();
					}

					contents.Add(materialResponse);
				}
				lessonResponses.Add(new LessonCourseResponse
				{
					Id = lesson.Id,
					Index = lesson.Index,
					Name = lesson.Name,
					TotalTime = lesson.TotalTime,
					Contents = contents
				});
			}
			courseResponse.ListLesson = lessonResponses.OrderBy(c => c.Index).ToList();

			courseResponse.ListTag = course.Tags?.Select(tag => new TagResponse
			{
				Id = tag.Id,
				Name = tag.Name,
				Type = tag.Type,
			}).ToList() ?? new List<TagResponse>();

			return apiResponse = GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, courseResponse, "name", $"course ID {request.CourseId}");
		}
	}
}
