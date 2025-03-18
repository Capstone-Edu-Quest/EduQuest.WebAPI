using AutoMapper;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
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
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICouponRepository _couponRepository;

		public GetCartByUserIdQueryHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork, ICouponRepository couponRepository)
		{
			_cartRepository = cartRepository;
			_unitOfWork = unitOfWork;
			_couponRepository = couponRepository;
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
