using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Stripe.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Infrastructure.Repository;

internal class ReportRepository : GenericRepository<Report>, IReportRepository
{
    private readonly ApplicationDbContext _context;
    public ReportRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<int> TotalPendingReports()
    {
        return await _context.Reports
            .Where(r => r.Status == (int)ReportStatus.Pending)
            .CountAsync();
    }
}
