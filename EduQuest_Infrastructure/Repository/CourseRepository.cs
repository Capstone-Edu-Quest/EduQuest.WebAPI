using System.Collections;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Request;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Google.Api;
using Microsoft.EntityFrameworkCore;
using Nest;
using static EduQuest_Domain.Enums.GeneralEnums;

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

        public async Task<(List<Course> Courses, int TotalItems)> SearchCoursesAsync(SearchCourseRequestDto request, int pageNo, int eachPage)
        {
            var query = _context.Courses
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.KeywordName))
            {
                var keyword = ContentHelper.ConvertVietnameseToEnglish(request.KeywordName.ToLower());
                query = query.Where(x =>
                    ContentHelper.ConvertVietnameseToEnglish(x.Title.ToLower()).Contains(keyword));
            }

            if (request.DateFrom.HasValue)
                query = query.Where(x => x.UpdatedAt >= request.DateFrom.Value);

            if (request.DateTo.HasValue)
                query = query.Where(x => x.UpdatedAt <= request.DateTo.Value);

            if (request.IsPublic.HasValue)
            {
                var isPublic = request.IsPublic.Value;
                query = query.Where(x =>
                    isPublic ? x.Status == StatusCourse.Public.ToString() : x.Status != StatusCourse.Public.ToString());
            }

            if (!string.IsNullOrWhiteSpace(request.Author))
            {
                var author = ContentHelper.ConvertVietnameseToEnglish(request.Author.ToLower());
                query = query.Where(x =>
                    ContentHelper.ConvertVietnameseToEnglish(x.User.Username.ToLower()).Contains(author));
            }

            if (request.Rating.HasValue)
                query = query.Where(x => x.CourseStatistic.Rating >= request.Rating.Value);

            if (request.TagListId != null && request.TagListId.Any())
            {
                var tagList = request.TagListId;
                query = query.Where(x => x.Tags.Any(tag => tagList.Contains(tag.Id)));
            }

            if (request.Sort != null)
            {
                var sortType = (SortCourse)request.Sort;
                query = sortType switch
                {
                    SortCourse.NewestCourses => query.OrderByDescending(x => x.CreatedAt),
                    SortCourse.MostReviews => query.OrderByDescending(x => x.CourseStatistic.TotalReview),
                    SortCourse.HighestRated => query.OrderByDescending(x => x.CourseStatistic.Rating),
                    _ => query.OrderByDescending(x => x.CreatedAt)
                };
            }

            var totalItems = await query.CountAsync();
            var pagedCourses = await query
                .Skip((pageNo - 1) * eachPage)
                .Take(eachPage)
                .ToListAsync();

            return (pagedCourses, totalItems);
        }




        public async Task<Course> GetCourseById(string Id)
        {
            return await _context.Courses.Include(x => x.Lessons).Include(x => x.User).Include(x => x.Tags).Include(x => x.CourseStatistic).Include(x => x.CourseLearners).FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<Course>> GetCourseByStatus(string status)
        {
            return await _context.Courses.Include(x => x.Lessons).Include(x => x.User).Include(x => x.Tags).Include(x => x.CourseStatistic).Where(x => x.Status.ToUpper().Equals(status.ToUpper())).ToListAsync();
        }

        public async Task<List<Course>> GetCourseByUserId(string Id)
        {
            return await _context.Courses.Include(x => x.Lessons).Include(x => x.Tags).Include(x => x.CourseStatistic).Where(x => x.CreatedBy == Id).ToListAsync();
        }

        public Task<List<Course>> GetCoursesByKeywordsAsync(List<string> keywords)
        {
            return _context.Courses.Where(course => keywords.Any(keyword => ContentHelper.ConvertVietnameseToEnglish(course.Title.ToLower()).Contains(ContentHelper.ConvertVietnameseToEnglish(keyword.ToLower())))).ToListAsync();
        }

        public async Task<int> GetTotalTime(string courseId)
        {
            var result = await _context.Courses.Include(x => x.CourseStatistic).FirstOrDefaultAsync(c => c.Id == courseId);
            var statistic = result.CourseStatistic;
            if (statistic.TotalTime == null)
            {
                return 0;
            }
            return result.CourseStatistic.TotalTime!.Value;
        }
        public async Task<bool> IsExist(string courseId)
        {
            return await _context.Courses.AnyAsync(c => c.Id == courseId);
        }

        public async Task<bool> IsOwner(string courseId, string UserId)
        {
            return await _context.Courses
            .AnyAsync(c => c.Id == courseId && c.CreatedBy == UserId);
        }
        public async Task<List<Tag>> GetTagByCourseIds(List<string> courseIds)
        {
            var tags = await _context.Tags
            .Where(t => t.Courses.Any(c => courseIds.Contains(c.Id)))
            .Distinct()
            .ToListAsync();

            return tags;
        }

        public async Task<List<Course>?> GetByListIds(List<string> courseIds)
        {
            var result = await _context.Courses
                .Where(c => courseIds.Contains(c.Id))
                .ToListAsync();
            return result;
        }

        public async Task<List<Course>> GetListCourse()
        {
            return await _context.Courses.Include(x => x.User).Include(x => x.Tags).ToListAsync();
        }

        public async Task<Course> GetCourseLearnerByCourseId(string courseId)
        {
            return await _context.Courses.Include(x => x.CourseLearners).Include(x => x.CourseStatistic).FirstOrDefaultAsync(x => x.Id == courseId);
        }

        public async Task<Course> GetCourseByUserIdAndCourseId(string userId, string courseId)
        {
            return await _context.Courses.FirstOrDefaultAsync(x => x.CreatedBy == userId && x.Id == courseId);

        }

        public async Task<int> GetCourseCountByCourseIdAsync(string courseId)
        {
            var cartCount = await _context.CartItems
            .Where(cart => cart.CourseId == courseId)
            .CountAsync();

            var transactionCount = await _context.TransactionDetails
                .Where(t => t.ItemType.ToLower() == GeneralEnums.ItemTypeTransactionDetail.Course.ToString().ToLower() && t.ItemId == courseId)
                .CountAsync();

            var totalCount = cartCount + transactionCount;

            return totalCount;
        }
        public async Task<AdminDashboardCourses> GetAdminDashBoardStatistic()
        {
            AdminDashboardCourses result = new AdminDashboardCourses();
            var Courses = _context.Courses.AsQueryable();

            result.TotalCourses = await Courses.CountAsync();

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            result.NewCoursesThisMonth = await Courses
                .Where(c => c.CreatedAt!.Value.Month == currentMonth && c.CreatedAt.Value.Year == currentYear)
                .CountAsync();
            var topCategory = await Courses
                .SelectMany(c => c.Tags!)
                .GroupBy(t => t.Name)
                .OrderByDescending(g => g.Count())
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .FirstOrDefaultAsync();
            if (topCategory == null)
            {
                result.MostPopularCategory = "NONE";
            }
            else
            {
                result.MostPopularCategory = topCategory.Category;
            }

            return result;
        }

        public async Task<List<Course>> GetCoursesByAssignToAsync(string expertId)
        {
            return await _context.Courses.AsNoTracking()
                .Where(c => c.AssignTo == expertId && c.Status == "Pending")
                .ToListAsync();
        }


        public async Task<List<Course>> GetCoursesByInstructorIdAsync(string instructorId)
        {
            return await _context.Courses.AsNoTracking()
                .Where(c => c.CreatedBy == instructorId)
                .ToListAsync();
        }

		public async Task<Course> GetCourseWithLessonsAndMaterialsAsync(string courseId)
		{
			return await _context.Courses
		       .Include(c => c.Lessons.OrderBy(l => l.Index))
			   .ThenInclude(l => l.LessonMaterials.OrderBy(lm => lm.Index))
			   .FirstOrDefaultAsync(c => c.Id == courseId);
		}


        public async Task UpdateStatus(string status, string courseId)
        {
             await _context.Courses
                .Where(a => a.Id == courseId)
                .ExecuteUpdateAsync(a => a.SetProperty(b => b.Status, status));
        }
    }

}

