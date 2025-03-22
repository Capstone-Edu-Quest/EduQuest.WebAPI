using AutoMapper;
using EduQuest_Application.DTO.Response;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;

namespace EduQuest_Application.UseCases.Courses.Queries.GetCourseById
{
	public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly IUserMetaRepository _userStatisticRepository;

		public GetCourseByIdQueryHandler(ICourseRepository courseRepository, IUserRepository userRepository, IMapper mapper, IUserMetaRepository userStatisticRepository)
		{
			_courseRepository = courseRepository;
			_userRepository = userRepository;
			_mapper = mapper;
			_userStatisticRepository = userStatisticRepository;
		}

		public async Task<APIResponse> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
		{
			var course = await _courseRepository.GetCourseById(request.CourseId);
			
			var courseResponse = _mapper.Map<CourseDetailResponse>(course);
			courseResponse.TotalLearner = course.CourseStatistic.TotalLearner;
			courseResponse.TotalReview = course.CourseStatistic.TotalReview;
			courseResponse.Rating = course.CourseStatistic.Rating;
			
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

			courseResponse.ListStage = course.Stages?
				.Select(stage => new StageCourseResponse
				{
					Level = stage.Level,
					Name = stage.Name,
					TotalTime = stage.TotalTime,
				})
				.ToList() ?? new List<StageCourseResponse>();

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
