using AutoMapper;
using EduQuest_Application.DTO.Response;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

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
			var result = _mapper.Map<List<UserResponseTestDto>>(users);	
			return new APIResponse
			{
				IsError = false,
				Payload = result,
				Errors = null,
			};
		}
	}
}
