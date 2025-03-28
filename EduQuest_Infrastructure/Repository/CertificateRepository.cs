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

    public async Task<PagedList<Certificate>> GetCertificatesWithFilters(
    string? title, string? userId, string? courseId, int page, int eachPage)
    {
        var query = _context.Certificates.AsQueryable();

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(c => c.Title.Contains(title));
        }

        if (!string.IsNullOrEmpty(courseId))
        {
            query = query.Where(c => c.CourseId == courseId);
        }

 

        int totalCount = await query.CountAsync();

        var items = await query.Skip((page - 1) * eachPage)
                               .Take(eachPage)
                               .ToListAsync();

        return new PagedList<Certificate>(items, totalCount, page, eachPage);
    }

}
