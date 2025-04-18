using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface ICertificateRepository : IGenericRepository<Certificate>
{
    Task BulkCreateAsync(List<Certificate> certificates);
    Task<List<Certificate>> GetCertificatesWithFilters(
    string? id, string? userId, string? courseId);
    Task<Certificate> GetByUserIdAndCourseId(string courseId, string userId);   
}
