using EduQuest_Application.DTO.Response.UserStatistics;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<List<User>?> GetByUserIds(List<string> ids);
    Task<User> GetUserById(string userId);
    Task<bool> UpdateUserPackageAccountType(string userId);
    Task<List<User>> GetByRoleId(string roleId);

    //Dashboard
    Task<AdminDasboardUsers> GetAdminDashBoardStatistic();
}
