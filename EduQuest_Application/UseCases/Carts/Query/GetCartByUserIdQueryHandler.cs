using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Carts.Query
{
	public class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, APIResponse>
	{
		private readonly ICartRepository _cartRepository;
		private readonly IMapper _mapper;

		public GetCartByUserIdQueryHandler(ICartRepository cartRepository, IMapper mapper)
		{
			_cartRepository = cartRepository;
			_mapper = mapper;
		}
		public async Task<APIResponse> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
		{
		
			var cart = await _cartRepository.GetByUserId(request.UserId);

			return new APIResponse
			{
				IsError = false,
				Payload = cart,
				Errors = null,
				Message = new MessageResponse
				{
					content = MessageCommon.GetSuccesfully,
					values = new Dictionary<string, string> { { "name", "cart" } }
				}
			};

			
		}
	}
}
