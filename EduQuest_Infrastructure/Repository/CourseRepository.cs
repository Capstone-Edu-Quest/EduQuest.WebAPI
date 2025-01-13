using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository
{
	public class CourseRepository : GenericRepository<Course>, ICourseRepository
	{
		private readonly ApplicationDbContext _context;

		public CourseRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Course> GetCourseById(string Id)
		{
			return await _context.Courses.Include(x => x.Stages).Include(x => x.User).FirstOrDefaultAsync(x => x.Id == Id);
		}
	}
}
