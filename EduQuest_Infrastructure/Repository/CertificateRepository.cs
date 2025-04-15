using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

public class CertificateRepository : GenericRepository<Certificate>, ICertificateRepository
{
    private readonly ApplicationDbContext _context;

    public CertificateRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Certificate>> GetCertificatesWithFilters(
    string? id, string? userId, string? courseId)
    {
        var query = _context.Certificates.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(id))
        {
            query = query.Where(c => c.Title.Contains(id));
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
