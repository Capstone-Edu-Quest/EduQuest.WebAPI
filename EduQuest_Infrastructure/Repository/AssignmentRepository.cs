using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository
{
	public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
	{
		private readonly ApplicationDbContext _context;

		public AssignmentRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<Assignment>> GetByUserId(string userId)
		{
			return await _context.Assignments.Where(x => x.UserId == userId).ToListAsync();
		}
	}
}
