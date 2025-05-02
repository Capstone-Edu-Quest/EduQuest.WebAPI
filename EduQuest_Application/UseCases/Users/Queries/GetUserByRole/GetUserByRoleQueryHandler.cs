using AutoMapper;
using EduQuest_Application.DTO.Response.Users;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Users.Queries.GetUserByRole
{
	public class GetUserByRoleQueryHandler : IRequestHandler<GetUserByRoleQuery, APIResponse>
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public GetUserByRoleQueryHandler(IUserRepository userRepository, IMapper mapper)
		{
			_userRepository = userRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(GetUserByRoleQuery request, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetByRoleId(request.RoleId, request.TagId);
			var response = _mapper.Map<List<UserBasicResponseDto>>(user);
			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, response, "name", "User with Role ${request.RoleId}");
		}
	}
}
