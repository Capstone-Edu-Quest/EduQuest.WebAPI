using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Infrastructure.Repository
{
	public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
	{
		private readonly ApplicationDbContext _context;

		public CartItemRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task DeleteCartItemByCartId(string cartId)
		{
			var cartItems = await _context.CartItems.Where(c => c.CartId == cartId).ToListAsync();

			foreach(var cartItem in cartItems)
			{
				_context.CartItems.Remove(cartItem);
			}
		}
	}
}
