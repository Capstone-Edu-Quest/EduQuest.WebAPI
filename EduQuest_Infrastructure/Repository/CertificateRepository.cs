using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;

namespace EduQuest_Infrastructure.Repository;

public class CertificateRepository : GenericRepository<Certificate>, ICertificateRepository
{
    private readonly ApplicationDbContext _context;

    public CertificateRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public IEnumerable<Certificate> GetCertificatesWithFilters(string? title, string? userId, string? courseId)
    {
        var query = _context.Certificates.AsQueryable();

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(c => c.Title.Contains(title));
        }
        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(c => c.UserId == userId);
        }
        if (!string.IsNullOrEmpty(courseId))
        {
            query = query.Where(c => c.CourseId == courseId);
        }

        return query.ToList();
    }
}
