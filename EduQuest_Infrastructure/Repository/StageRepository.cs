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
	public class StageRepository : GenericRepository<Stage>, IStageRepository
	{
		private readonly ApplicationDbContext _context;

		public StageRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<bool> DeleteStagesByCourseId(string courseId)
		{
			var stages = _context.Stages.Where(x => x.CourseId == courseId).ToList();

			if(stages.Any())
			{
				_context.Stages.RemoveRange(stages);
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}

		public async Task<List<Stage>> GetByCourseId(string id)
		{
			return await _context.Stages.Where(x => x.CourseId.Equals(id)).ToListAsync();
		}

		public async Task<int?> GetMaxLevelInThisCourse(string id)
		{
			return await _context.Stages.MaxAsync(s => (int?)s.Level);
		}
	}
}
