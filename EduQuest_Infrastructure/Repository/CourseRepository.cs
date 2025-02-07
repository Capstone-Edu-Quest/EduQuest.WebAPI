using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace EduQuest_Infrastructure.Repository
{
	public class CourseRepository : GenericRepository<Course>, ICourseRepository
	{
		private readonly ApplicationDbContext _context;

		public CourseRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		//public async Task<IEnumerable<Course>> GetAllCourse()
		//{
		//	return await _context.Courses.Include(x => x.User).Include(x => x.CourseStatistic).ToListAsync();
		//}

		public async Task<Course> GetCourseById(string Id)
		{
			return await _context.Courses.Include(x => x.Stages).Include(x => x.User).Include(x => x.Tags).Include(x => x.CourseStatistic).FirstOrDefaultAsync(x => x.Id == Id);
		}

		public async Task<IEnumerable<Course>> GetCourseByUserId(string Id)
		{
			return await _context.Courses.Include(x => x.Stages).Include(x => x.Tags).Where(x => x.CreatedBy == Id).ToListAsync();
		}


        public async Task<int> GetTotalTime(string courseId)
		{
			var result = await _context.Courses.Include(x => x.CourseStatistic).FirstOrDefaultAsync(c => c.Id == courseId);
			var statistic = result.CourseStatistic;
			if(statistic.TotalTime == null)
			{
				return 0;
			}
			return result.CourseStatistic.TotalTime.Value;
		}
		public async Task<bool> IsExist(string courseId)
		{
			var result = await _context.Courses.FindAsync(courseId);
			return result != null ? true : false;
		}
    }
}
