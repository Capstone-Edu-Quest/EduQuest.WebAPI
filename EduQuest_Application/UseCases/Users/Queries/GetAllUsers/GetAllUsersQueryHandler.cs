using AutoMapper;
using EduQuest_Application.DTO.Response.Profiles;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, APIResponse>
	{
		private readonly IMapper _mapper;
		private readonly IUserRepository _userRepo;
		private readonly ICourseRepository _courseRepository;

        public GetAllUsersQueryHandler(IMapper mapper, IUserRepository userRepo, ICourseRepository courseRepository)
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _courseRepository = courseRepository;
        }

        public async Task<APIResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var existUsers = await _userRepo.GetUserByStatus(request.Status);

            if (!existUsers.Any())
            {
                return new APIResponse
                {
                    IsError = false,
                    Payload = null,
                    Errors = null,
                    Message = new MessageResponse { content = MessageCommon.GetSuccesfully }
                };
            }

            var instructorDtos = new List<InstructorProfileDto>();

            foreach (var user in existUsers)
            {
                var courses = await _courseRepository.GetCoursesByInstructorIdAsync(user.Id);
                var courseDtos = _mapper.Map<List<CourseProfileDto>>(courses);

                int totalLearners = courses.Sum(c => c.CourseLearners?.Count ?? 0);
                int totalReviews = courses.Sum(c => c.Feedbacks?.Count ?? 0);

                var instructorDto = _mapper.Map<InstructorProfileDto>(user);
                instructorDto.Courses = courseDtos;
                instructorDto.TotalLearners = totalLearners;
                instructorDto.TotalReviews = totalReviews;

                if (!string.IsNullOrEmpty(user.AssignToExpertId))
                {
                    var expertUser = await _userRepo.GetById(user.AssignToExpertId);
                    if (expertUser != null)
                    {
                        instructorDto.ExpertName = expertUser.Username;
                    }
                }

                instructorDtos.Add(instructorDto);
            }


            return new APIResponse
            {
                IsError = false,
                Payload = instructorDtos,
                Errors = null,
                Message = new MessageResponse { content = MessageCommon.GetSuccesfully }
            };
        }

    }
}
