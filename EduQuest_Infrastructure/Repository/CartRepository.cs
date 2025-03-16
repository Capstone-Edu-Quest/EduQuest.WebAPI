using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
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
	public class CartRepository : GenericRepository<Cart>, ICartRepository
	{
		private readonly ApplicationDbContext _context;

		public CartRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Cart> GetByUserId(string userId)
		{
			return await _context.Carts.Include(x => x.CartItems).FirstOrDefaultAsync(x => x.UserId.Equals(userId));
		}

		public async Task<Cart> GetListCartItemByCartId(string cartId)
		{
			return await _context.Carts.Include(x => x.CartItems).FirstOrDefaultAsync(x => x.Id.Equals(cartId));
		}
	}
}
