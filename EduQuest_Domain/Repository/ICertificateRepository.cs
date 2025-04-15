using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface ICertificateRepository : IGenericRepository<Certificate>
{
    Task<List<Certificate>> GetCertificatesWithFilters(
    string? id, string? userId, string? courseId);
}
