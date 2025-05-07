using AutoMapper;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.Lessons;
using EduQuest_Application.DTO.Response.Materials;
using EduQuest_Application.DTO.Response.Tags;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Xml;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.Courses.Query.GetCourseDetailForIntructor
{
    public class GetCourseDetailForIntructorQueryHandler : IRequestHandler<GetCourseDetailForIntructorQuery, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly ILessonRepository _lessonRepository;
		private readonly ILearnerRepository _learnerRepository;
		private readonly IMaterialRepository _materialRepository;
		private readonly IFavoriteListRepository _favoriteListRepository;
		private readonly IFeedbackRepository _feedbackRepository;
		private readonly IMapper _mapper;

		public GetCourseDetailForIntructorQueryHandler(ICourseRepository courseRepository, 
			ILessonRepository lessonRepository, 
			ILearnerRepository learnerRepository, 
			IMaterialRepository materialRepository, 
			IFavoriteListRepository favoriteListRepository, 
			IFeedbackRepository feedbackRepository, 
			IMapper mapper)
		{
			_courseRepository = courseRepository;
			_lessonRepository = lessonRepository;
			_learnerRepository = learnerRepository;
			_materialRepository = materialRepository;
			_favoriteListRepository = favoriteListRepository;
			_feedbackRepository = feedbackRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(GetCourseDetailForIntructorQuery request, CancellationToken cancellationToken)
		{
			var apiResponse = new APIResponse();

			var existedCourse = await _courseRepository.GetCourseByUserIdAndCourseId(request.UserId, request.CourseId);
			var courseResponse = _mapper.Map<CourseDetailResponseForIntructor>(existedCourse);
			courseResponse.RequirementList =  existedCourse.Requirement != null ?  ContentHelper.SplitString(existedCourse.Requirement, '.') : null;
			
			courseResponse.LastUpdated = existedCourse.UpdatedAt;
			courseResponse.TotalLearner = existedCourse.CourseStatistic.TotalLearner;
			courseResponse.TotalReview = existedCourse.CourseStatistic.TotalReview;
			courseResponse.Rating = existedCourse.CourseStatistic.Rating;
			courseResponse.TotalTime = existedCourse.CourseStatistic.TotalTime;
			courseResponse.TotalLesson = existedCourse.CourseStatistic.TotalLesson;
			courseResponse.TotalInCart = await _courseRepository.GetCourseCountByCourseIdAsync(request.CourseId);
			courseResponse.TotalInWishList = await _favoriteListRepository.GetCountByCourseId(request.CourseId);
			courseResponse.CourseEnrollOverTime = await _learnerRepository.GetCourseEnrollOverTimeAsync(request.CourseId);
			courseResponse.CourseRatingOverTime = await _feedbackRepository.GetCourseRatingOverTimeAsync(request.CourseId);
			courseResponse.IsPublic = existedCourse.Status == StatusCourse.Public.ToString() ? true : false;

            var lessonResponses = new List<LessonCourseResponse>();

			foreach (var lesson in existedCourse.Lessons!)
			{
				var lessonInCourse = await _lessonRepository.GetByLessonIdAsync(lesson.Id);

				var materials = new List<MaterialInLessonResponse>();

				var listMaterialId = lessonInCourse.LessonMaterials.OrderBy(x=> x.Index).Select(x => x.MaterialId).Distinct().ToList();
				var listMaterial = await _materialRepository.GetMaterialsByIds(listMaterialId);
				var sortedMaterials = listMaterial
					.OrderBy(m => listMaterialId.IndexOf(m.Id))
					.ToList();

				foreach (var material in sortedMaterials)
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
			courseResponse.ListLesson = lessonResponses.OrderBy(l => l.Index).ToList();
			courseResponse.ListTag = existedCourse.Tags?.Select(tag => new TagResponse
			{
				Id = tag.Id,
				Name = tag.Name,
				Type = tag.Type,
			}).ToList() ?? new List<TagResponse>();
			return apiResponse = GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, courseResponse, "name", $"course ID {courseResponse.Id}");
		}
	}
}
