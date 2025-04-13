using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.CartItems.Command
{
	public class AddCartItemCommandHandler : IRequestHandler<AddCartItemCommand, APIResponse>
	{
		private readonly ICartRepository _cartRepository;
		private readonly ICartItemRepository _cartItemRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICourseRepository _courseRepository;
        private readonly ILearnerRepository _learnerRepository;

		public AddCartItemCommandHandler(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IUnitOfWork unitOfWork, ICourseRepository courseRepository, ILearnerRepository learnerRepository)
		{
			_cartRepository = cartRepository;
			_cartItemRepository = cartItemRepository;
			_unitOfWork = unitOfWork;
			_courseRepository = courseRepository;
			_learnerRepository = learnerRepository;
		}

		public async Task<APIResponse> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByUserId(request.UserId);

            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = request.UserId,
                    Total = 0
                };
                await _cartRepository.Add(cart);
            }
            else
            {
                await _cartItemRepository.DeleteCartItemByCartId(cart.Id);
                cart.Total = 0;
                await _cartRepository.Update(cart);
            }

            var cartItems = new List<CartItem>();
            var myCourseStudying = await _learnerRepository.GetCoursesIdStudying(request.UserId);

			if (request.CourseIds != null && request.CourseIds.Any())
            {
				foreach (var courseId in request.CourseIds)
                {
                    if (myCourseStudying.Contains(courseId))
                    {
						return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, Constants.MessageError.CourseExist, MessageError.CourseExist, "name", $"Course ID {courseId}");
					}
                    var course = await _courseRepository.GetById(courseId);

                    if (course == null)
                        continue; 

                    var cartItem = new CartItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        CartId = cart.Id,
                        CourseId = courseId,
                        Price = (decimal)course.Price
                    };

                    cart.Total += cartItem.Price;
                    cartItems.Add(cartItem);
                }

                await _cartItemRepository.CreateRangeAsync(cartItems);
            }

            await _cartRepository.Update(cart);

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return GeneralHelper.CreateSuccessResponse(
                    HttpStatusCode.OK,
                    MessageCommon.UpdateSuccesfully,
                    cartItems,
                    "name",
                    "Add to cart"
                );
            }

            return GeneralHelper.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                MessageCommon.UpdateFailed,
                "Saving Failed",
                "name",
                "Add to cart"
            );
        }

    }
}
