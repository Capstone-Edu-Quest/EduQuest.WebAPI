using AutoMapper;
using EduQuest_Application.DTO.Response;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, APIResponse>
	{
		private readonly IMapper _mapper;
		private readonly IUserRepository _userRepo;

		public GetAllUsersQueryHandler(IMapper mapper, IUserRepository userRepo)
		{
			_mapper = mapper;
			_userRepo = userRepo;
		}

		public async Task<APIResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
		{
			var users = await _userRepo.GetAll();
			var result = _mapper.Map<List<UserResponseDto>>(users);	
			return new APIResponse
			{
				IsError = false,
				Payload = result,
				Errors = null,
				//Message = new MessageResponse
				//{
				//	content = "fawf",
    //                values = users.Take(1)
    //                     .Select((user, index) => new { Key = $"name", Value = user.Username })
    //                     .ToDictionary(x => x.Key, x => x.Value)
					
    //            }
			};
		}
	}
}
