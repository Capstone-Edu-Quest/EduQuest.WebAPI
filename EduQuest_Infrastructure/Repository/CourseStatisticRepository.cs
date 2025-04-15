using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.User;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository
{
	public class CourseStatisticRepository : GenericRepository<CourseStatistic>, ICourseStatisticRepository
	{
		private readonly ApplicationDbContext _context;

		public CourseStatisticRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<CourseStatistic> GetByCourseId(string courseId)
		{
			return await _context.CourseStatistics.FirstOrDefaultAsync(x => x.CourseId.Equals(courseId));
		}

		public async Task<StatisticForInstructor> GetStatisticForInstructor(List<string> courseIds)
		{
			throw new NotImplementedException();
		}
	}
}
