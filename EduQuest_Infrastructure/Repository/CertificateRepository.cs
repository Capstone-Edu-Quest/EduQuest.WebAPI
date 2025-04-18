using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;


namespace EduQuest_Infrastructure.Repository;

public class CertificateRepository : GenericRepository<Certificate>, ICertificateRepository
{
    private readonly ApplicationDbContext _context;

    public CertificateRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task BulkCreateAsync(List<Certificate> certificates)
    {
        await _context.BulkInsertAsync(certificates);
    }

	public async Task<Certificate> GetByUserIdAndCourseId(string courseId, string userId)
	{
		return await _context.Certificates.FirstOrDefaultAsync(x => x.CourseId == courseId && x.UserId == userId);
	}

	public async Task<List<Certificate>> GetCertificatesWithFilters(
    string? id, string? userId, string? courseId)
    {
        var query = _context.Certificates.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(id))
        {
            query = query.Where(c => c.Id.Equals(id));
        }

        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(c => c.UserId == userId);
        }

        if (!string.IsNullOrEmpty(courseId))
        {
            query = query.Where(c => c.CourseId == courseId);
        }

        return await query.ToListAsync();
    }

}
