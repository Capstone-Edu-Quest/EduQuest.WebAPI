using AutoMapper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Command.CreateCourse
{
	public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;	
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserRepository _userRepository;
		private readonly IUserMetaRepository _userStatisticRepository;
		

		public CreateCourseCommandHandler(ICourseRepository courseRepository, 
			IMapper mapper, 
			IUnitOfWork unitOfWork, 
			IUserRepository userRepository, 
			IUserMetaRepository userStatisticRepository 
			)
		{
			_courseRepository = courseRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_userRepository = userRepository;
			_userStatisticRepository = userStatisticRepository;
			
		}

		public async Task<APIResponse> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
		{
			var course = _mapper.Map<Course>(request.CourseRequest);
			
			var user = await _userRepository.GetById(request.UserId);
			
			course.CreatedBy = user!.Id;
			course.Id = Guid.NewGuid().ToString();
			//course.LastUpdated = DateTime.Now.ToUniversalTime();
			course.Status = GeneralEnums.StatusCourse.Draft.ToString();
			course.IsRequired = false;
			course.CourseStatistic = new CourseStatistic
			{
				Id = Guid.NewGuid().ToString(),
				CourseId = course.Id,
				TotalLearner = 0,
				TotalLesson = 0,
				TotalReview = 0,
				Rating = 0,
				TotalTime = 0
			};
			await _courseRepository.Add(course);
			
			//User Statistic
			var userStatistic = await _userStatisticRepository.GetByUserId(user.Id);
			userStatistic!.TotalCourseCreated++;
			await _userStatisticRepository.Update(userStatistic);

			var result = await _unitOfWork.SaveChangesAsync() > 0;
			return new APIResponse
			{
				IsError = !result,
				Payload = result ? course : null,
				Errors = result ? null : new ErrorResponse
				{
					StatusResponse = HttpStatusCode.BadRequest,
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = MessageCommon.SavingFailed,
				},
				Message = new MessageResponse
				{
					content = result ? MessageCommon.CreateSuccesfully : MessageCommon.CreateFailed,
					values = new Dictionary<string, string> { { "name", "course" } }
				}
			};

		


		}
	}
}
