using AutoMapper;
using EduQuest_Application.UseCases.Courses.Cpmmands.CreateCourse;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using System.Runtime.InteropServices;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Command.CreateCourse
{
	public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserRepository _userRepository;

		public CreateCourseCommandHandler(ICourseRepository courseRepository, IMapper mapper, IUnitOfWork unitOfWork,
			IUserRepository userRepository)
		{
			_courseRepository = courseRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_userRepository = userRepository;
		}

		public async Task<APIResponse> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
		{
			var course = _mapper.Map<Course>(request.CourseRequest);
			
			var user = await _userRepository.GetById(request.UserId);
			
			course.CreatedBy = user.Id;
			course.Id = Guid.NewGuid().ToString();
			course.LastUpdated = DateTime.Now;
			await _courseRepository.Add(course);
			//await _unitOfWork.SaveChangesAsync();
			return await _unitOfWork.SaveChangesAsync() > 0 ? new APIResponse
			{
				IsError = false,
				Payload = course,
				Errors = null,
			} : new APIResponse
			{
				IsError = true,
				Payload = null,
				Errors = new ErrorResponse
				{
					StatusResponse = HttpStatusCode.BadRequest, // Use appropriate HTTP status code
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = MessageCommon.SavingFailed,
				}
			};

		}
	}
}
