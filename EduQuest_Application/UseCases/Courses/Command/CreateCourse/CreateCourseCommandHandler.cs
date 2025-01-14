using AutoMapper;
using EduQuest_Application.UseCases.Courses.Cpmmands.CreateCourse;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;

namespace EduQuest_Application.UseCases.Courses.Command.CreateCourse
{
	public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public CreateCourseCommandHandler(ICourseRepository courseRepository, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_courseRepository = courseRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<APIResponse> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
		{
			var course = _mapper.Map<Course>(request.CourseRequest);
			course.CreatedBy = "A84B8BBD-73E0-4857-85A8-C16136C214C8"; //A84B8BBD-73E0-4857-85A8-C16136C214C8
			course.Id = Guid.NewGuid().ToString();
			course.LastUpdated = DateTime.Now;
			await _courseRepository.Add(course);
			await _unitOfWork.SaveChangesAsync();
			return new APIResponse
			{
				IsError = false,
				Payload = course,
				Errors = null,
			};

		}
	}
}
