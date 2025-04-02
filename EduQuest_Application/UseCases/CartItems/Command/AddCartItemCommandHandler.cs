using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
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

namespace EduQuest_Application.UseCases.CartItems.Command
{
	public class AddCartItemCommandHandler : IRequestHandler<AddCartItemCommand, APIResponse>
	{
		private readonly ICartRepository _cartRepository;
		private readonly ICartItemRepository _cartItemRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICourseRepository _courseRepository;

		public AddCartItemCommandHandler(ICartRepository cartRepository, 
			ICartItemRepository cartItemRepository, 
			IUnitOfWork unitOfWork, 
			ICourseRepository courseRepository)
		{
			_cartRepository = cartRepository;
			_cartItemRepository = cartItemRepository;
			_unitOfWork = unitOfWork;
			_courseRepository = courseRepository;
		}

		public async Task<APIResponse> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
		{
			var response = new APIResponse();
			var newCart = new Cart();
			var cart = await _cartRepository.GetByUserId(request.UserId);

			if (cart != null && cart.CartItems.Any())
			{
				await _cartItemRepository.DeleteCartItemByCartId(cart.Id);
				await _cartRepository.Delete(cart.Id);

				newCart = new Cart
				{
					Id = Guid.NewGuid().ToString(),
					UserId = request.UserId,
					Total = 0
				};

				await _cartRepository.Add(newCart);
				await _unitOfWork.SaveChangesAsync();
			} 

			var cartItems = new List<CartItem>();
			var existedCart = await _cartRepository.GetByUserId(request.UserId);


			foreach (var item in request.CourseIds)
			{
				var course = await _courseRepository.GetById(item);
				var cartItem = new CartItem
				{
					Id = Guid.NewGuid().ToString(),
					CartId = existedCart!.Id,
					CourseId = item,
					Price = (decimal)course.Price,
				};

				existedCart.Total += cartItem.Price;
				cartItems.Add(cartItem);
			}

			await _cartItemRepository.CreateRangeAsync(cartItems);
			if (await _unitOfWork.SaveChangesAsync() > 0)
			{
				return GeneralHelper.CreateSuccessResponse(
					HttpStatusCode.OK,
					MessageCommon.CreateSuccesfully,
					cartItems,
					"name",
					"Add to cart"
				);
			}

			return GeneralHelper.CreateErrorResponse(
				HttpStatusCode.BadRequest,
				MessageCommon.CreateFailed,
				"Saving Failed",
				"name",
				"Add to cart"
			);
		}
	}
}
