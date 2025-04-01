using AutoMapper;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.Lessons;
using EduQuest_Application.DTO.Response.Materials;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;

namespace EduQuest_Application.UseCases.Courses.Queries.GetCourseById
{
	public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly ILessonRepository _lessonRepository;
		private readonly IMaterialRepository _materialRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly IUserMetaRepository _userStatisticRepository;

		public GetCourseByIdQueryHandler(ICourseRepository courseRepository, ILessonRepository lessonRepository, IMaterialRepository materialRepository, IUserRepository userRepository, IMapper mapper, IUserMetaRepository userStatisticRepository)
		{
			_courseRepository = courseRepository;
			_lessonRepository = lessonRepository;
			_materialRepository = materialRepository;
			_userRepository = userRepository;
			_mapper = mapper;
			_userStatisticRepository = userStatisticRepository;
		}

		public async Task<APIResponse> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
		{
			var course = await _courseRepository.GetCourseById(request.CourseId);
			var courseWithLearner = await _courseRepository.GetCourseLearnerByCourseId(request.CourseId);
			var courseLearner = courseWithLearner.CourseLearners!.FirstOrDefault(x => x.UserId == request.UserId);

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
			}
			
			courseResponse.Author = course.User! != null ? new AuthorCourseResponse
			{
				Id = course.User.Id,
				Username = course.User.Username ?? string.Empty,
				Headline = course.User.Headline ?? string.Empty,
				Description = course.User.Description ?? string.Empty
			} : null;

			var userSta = await _userStatisticRepository.GetByUserId(course.User!.Id);
			if(userSta != null)
			{
				courseResponse.Author!.TotalCourseCreated = userSta.TotalCourseCreated;
				courseResponse.Author.TotalReview = userSta.TotalReview;
				courseResponse.Author.TotalLearner = userSta.TotalLearner;
			}

			var lessonResponses = new List<LessonCourseResponse>();

			foreach (var lesson in course.Lessons!)
			{
				var lessonInCourse = await _lessonRepository.GetByLessonIdAsync(lesson.Id);

				var materials = new List<MaterialInLessonResponse>();

				
					foreach (var material in lessonInCourse.Materials)
					{
						var currentMaterialResponse = new MaterialInLessonResponse
						{
							Id = material.Id,
							Type = material.Type,
							Duration = material.Duration,
							Title = material.Title,
							Description = material.Description,
							Version = material.Version,
							OriginalMaterialId = material.OriginalMaterialId,
						};

						materials.Add(currentMaterialResponse);

						// Nếu có OriginalMaterialId, thì lấy thêm thông tin của Material gốc
						if (material.OriginalMaterialId != null)
						{
							var originalMaterial = await _materialRepository.GetById(material.OriginalMaterialId);

							if (originalMaterial != null)
							{
								var originalMaterialResponse = new MaterialInLessonResponse
								{
									Id = originalMaterial.Id,
									Type = originalMaterial.Type,
									Duration = originalMaterial.Duration,
									Title = originalMaterial.Title,
									Description = originalMaterial.Description,
									Version = originalMaterial.Version,
								};

								materials.Add(originalMaterialResponse);
							}
						}
					}
				
				lessonResponses.Add(new LessonCourseResponse
				{
					Id = lesson.Id,
					Index = lesson.Index,
					Name = lesson.Name,
					TotalTime = lesson.TotalTime,
					Materials = materials
				});
			}
			courseResponse.ListLesson = lessonResponses;

			courseResponse.ListTag = course.Tags?.Select(tag => new TagResponse
			{
				Name = tag.Name
			}).ToList() ?? new List<TagResponse>();

			//Chưa có data fb
			//Hoàn tiền

			return new APIResponse
			{
				IsError = false,
				Payload = courseResponse,
				Errors = null,
			};
		}
	}
}
