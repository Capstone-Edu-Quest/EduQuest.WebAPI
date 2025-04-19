using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Repository;

public class InstructorCertificateRepository : GenericRepository<InstructorCertificate>, IInstructorCertificate
{
    private readonly ApplicationDbContext _context;
    public InstructorCertificateRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task BulkCreateAsync(List<InstructorCertificate> certificates)
    {
        await _context.BulkInsertAsync(certificates);
    }

    public async Task<List<InstructorCertificate>> GetByUserIdAsync(string userId)
    {
        return await _context.InstructorCertificates
            .Where(c => c.UserId == userId).ToListAsync();
    }


}

