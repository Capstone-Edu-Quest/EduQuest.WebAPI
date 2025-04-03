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

}
