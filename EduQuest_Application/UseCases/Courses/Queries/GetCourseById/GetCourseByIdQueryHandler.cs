using AutoMapper;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.UseCases.Courses.Queries.SearchCourse;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Courses.Queries.GetCourseById
{
	public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;


		public async Task<APIResponse> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
		{
			var course = await _courseRepository.GetById(request.CourseId);
			var courseResponse = _mapper.Map<CourseDetailResponse>(course);

			
			
			

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
