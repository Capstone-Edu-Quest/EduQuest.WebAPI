using AutoMapper;
using EduQuest_Application.DTO.Response.Users;
using EduQuest_Domain.Models.Pagination;
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

		public GetAllUsersQueryHandler(IMapper mapper, IUserRepository userRepo)
		{
			_mapper = mapper;
			_userRepo = userRepo;
		}

		public async Task<APIResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
		{
			var users = await _userRepo.GetAll(request.Page, request.Pagesize);
			var result = _mapper.Map<PagedList<UserResponseDto>>(users);	
			return new APIResponse
			{
				IsError = false,
				Payload = result,
				Errors = null,
				Message = new MessageResponse { content = MessageCommon.GetSuccesfully}
			};
		}
	}
}
