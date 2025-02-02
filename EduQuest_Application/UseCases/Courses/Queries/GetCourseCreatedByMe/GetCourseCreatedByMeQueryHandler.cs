using AutoMapper;
using EduQuest_Application.DTO.Response;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;

namespace EduQuest_Application.UseCases.Courses.Queries.GetCourseCreatedByMe
{
	public class GetCourseCreatedByMeQueryHandler : IRequestHandler<GetCourseCreatedByMeQuery, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IMapper _mapper;

		public GetCourseCreatedByMeQueryHandler(ICourseRepository courseRepository, IMapper mapper)
		{
			_courseRepository = courseRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(GetCourseCreatedByMeQuery request, CancellationToken cancellationToken)
		{
			var courseList = await _courseRepository.GetCourseByUserId(request.UserId);

			var courseResponse = courseList.Select(course =>
			{
				var response = _mapper.Map<CourseDetailResponse>(course);
				response.ListStage = course.Stages?
					.Select(stage => new StageCourseResponse
					{
						Level = stage.Level,
						Name = stage.Name
					})
					.ToList() ?? new List<StageCourseResponse>();


				response.ListTag = course.Tags?
					.Select(tag => new TagResponse
					{
						Name = tag.Name
					})
					.ToList() ?? new List<TagResponse>();

				return response;
			}).ToList();
			return new APIResponse
			{
				IsError = false,
				Payload = courseResponse,
				Errors = null
			};


		}
	}
}
