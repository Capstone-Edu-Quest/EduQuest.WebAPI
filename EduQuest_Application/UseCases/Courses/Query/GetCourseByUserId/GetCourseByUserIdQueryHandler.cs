using AutoMapper;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.Lessons;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;

namespace EduQuest_Application.UseCases.Courses.Queries

{
	public class GetCourseByUserIdQueryHandler : IRequestHandler<GetCourseByUserIdQuery, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IMapper _mapper;

		public GetCourseByUserIdQueryHandler(ICourseRepository courseRepository, IMapper mapper)
		{
			_courseRepository = courseRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(GetCourseByUserIdQuery request, CancellationToken cancellationToken)
		{
			var courseList = new List<Course>();
			if(request.IntructorId == null)
			{
				courseList = await _courseRepository.GetCourseByUserId(request.UserId);

				var courseResponse = courseList.Select(course =>
				{
					var response = _mapper.Map<CourseDetailResponse>(course);
					response.RequirementList = ContentHelper.SplitString(course.Requirement, '.');
					response.ListLesson = course.Lessons?
						.Select(stage => new LessonCourseResponse
						{
							Index = stage.Index,
							Name = stage.Name
						})
						.ToList() ?? new List<LessonCourseResponse>();


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
			} else
			{
				courseList = await _courseRepository.GetCourseByUserId(request.IntructorId);
				var courseResponse = courseList.Select(course =>
				{
					var response = _mapper.Map<CourseDetailResponse>(course);
					response.RequirementList = ContentHelper.SplitString(course.Requirement, '.');
					response.ListLesson = course.Lessons?
						.Select(stage => new LessonCourseResponse
						{
							Index = stage.Index,
							Name = stage.Name
						})
						.ToList() ?? new List<LessonCourseResponse>();


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
}
