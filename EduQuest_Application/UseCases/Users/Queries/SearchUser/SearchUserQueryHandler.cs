using AutoMapper;
using EduQuest_Application.Helper;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;

namespace EduQuest_Application.UseCases.Users.Queries.SearchUser;



public class SearchUserQueryHandler : IRequestHandler<SearchUserQuery, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public SearchUserQueryHandler(
        IUserRepository userRepository,
        ICourseRepository courseRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(SearchUserQuery request, CancellationToken cancellationToken)
    {
        // Get users based on filters
        var users = await _userRepository.SearchUsersAsync(
            request.Username,
            request.Email,
            request.Phone,
            request.Status,
            request.RoleId);

        var userDtos = new List<UserSearchResultDto>();

        foreach (var user in users)
        {
            var userDto = _mapper.Map<UserSearchResultDto>(user);

            // Get courses by instructor
            var courses = await _courseRepository.GetCoursesByInstructorIdAsync(user.Id);
            if (courses != null)
            {
                userDto.TotalCourses = courses.Count;
                userDto.TotalLearners = courses.Sum(c => c.CourseLearners?.Count ?? 0);
                userDto.TotalReviews = courses.Sum(c => c.Feedbacks?.Count ?? 0);
            }

            // Get ExpertName if exists
            if (!string.IsNullOrEmpty(user.AssignToExpertId))
            {
                var expertUser = await _userRepository.GetById(user.AssignToExpertId);
                if (expertUser != null)
                {
                    userDto.ExpertName = expertUser.Username;
                }
            }

            userDtos.Add(userDto);
        }

        var response = new SearchUserResponse
        {
            Users = userDtos
        };

        return GeneralHelper.CreateSuccessResponse(
            HttpStatusCode.OK,
            Constants.MessageCommon.GetSuccesfully,
            response, "name", "");
    }
}



public class UserSearchResultDto : IMapFrom<User>, IMapTo<User>
{
    public string Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Status { get; set; }
    public string? RoleId { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Headline { get; set; }
    public string? Description { get; set; }
    public string? ExpertName { get; set; }
    public int TotalCourses { get; set; }
    public int TotalLearners { get; set; }
    public int TotalReviews { get; set; }

    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<User, UserSearchResultDto>();
    }
}

public class SearchUserResponse
{
    public List<UserSearchResultDto> Users { get; set; } = new List<UserSearchResultDto>();
}