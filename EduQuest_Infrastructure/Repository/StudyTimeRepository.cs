using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

public class StudyTimeRepository : GenericRepository<StudyTime>, IStudyTimeRepository
{
    private readonly ApplicationDbContext _context;

    public StudyTimeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IList<StudyTime>> GetStudyTimeByUserId(string userId)
    {
        return await _context.StudyTimes.AsNoTracking().Where(a => a.UserId == userId).ToListAsync();
    }

    public async Task<StudyTime?> GetByDate(DateTime date, string userId)
    {
        var result = await _context.StudyTimes
            .Where(s => s.Date.Year == date.Year && s.Date.Month == date.Month && s.Date.Day == date.Day && s.UserId == userId)
            .FirstOrDefaultAsync();
        return result;
    }

}
