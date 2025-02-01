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

		public GetCourseByIdQueryHandler(ICourseRepository courseRepository, IUserRepository userRepository, IMapper mapper)
		{
			_courseRepository = courseRepository;
			_userRepository = userRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
		{
			var course = await _courseRepository.GetById(request.CourseId);
			
			var courseResponse = _mapper.Map<CourseDetailResponse>(course);

			courseResponse.Author = course.User! != null ? new AuthorCourseResponse
			{
				Id = course.User.Id,
				Username = course.User.Username ?? string.Empty,
				Headline = course.User.Headline ?? string.Empty,
				Description = course.User.Description ?? string.Empty
			} : null;


			courseResponse.ListStage = course.Stages?
				.Select(stage => new StageCourseResponse
				{
					Level = stage.Level,
					Name = stage.Name
				})
				.ToList() ?? new List<StageCourseResponse>();

			courseResponse.ListTag = course.Tags?.Select(tag => new TagResponse
			{
				Name = tag.Name
			}).ToList() ?? new List<TagResponse>();

			//Chưa có rating, data fb
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
