using EduQuest_Application.DTO.Response;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.WebStatistics.Queries.AdminHomeDashboard;

internal class AdminHomeDashboardHandler : IRequestHandler<AdminHomeDashboardQuery, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IReportRepository _reportRepository;

    public AdminHomeDashboardHandler(IUserRepository userRepository, ICourseRepository courseRepository, 
        IReportRepository reportRepository)
    {
        _userRepository = userRepository;
        _courseRepository = courseRepository;
        _reportRepository = reportRepository;
    }

    public async Task<APIResponse> Handle(AdminHomeDashboardQuery request, CancellationToken cancellationToken)
    {
        AdminDashboard response = new AdminDashboard();
        response.AdminDashboardCourses = await _courseRepository.GetAdminDashBoardStatistic();
        response.AdminDasboardUsers = await _userRepository.GetAdminDashBoardStatistic();
        response.PendingViolations = await _reportRepository.TotalPendingReports();

        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.Complete,
            response, "name", "dashboard");
    }
}
