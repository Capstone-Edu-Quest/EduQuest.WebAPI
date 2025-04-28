using EduQuest_Domain.Entities;
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

        public async Task<(int totalLearner, decimal avgRating)> GetTotalLearnerForInstructor(List<string> courseIds)
        {
            var statistic = await _context.CourseStatistics
                .Where(x => courseIds.Contains(x.CourseId))
                .ToListAsync();

            // Check if the collection is empty to avoid calling Average() on an empty sequence
            var totalLearner = statistic.Sum(x => x.TotalLearner.GetValueOrDefault());

            // Safely calculate average or return 0 if the collection is empty
            decimal avgReview = (decimal)(statistic.Any() ? statistic.Average(x => x.Rating.GetValueOrDefault()) : 0);

            return (totalLearner, avgReview);
        }


    }
}
