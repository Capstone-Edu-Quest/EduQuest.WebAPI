using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface ICertificateRepository : IGenericRepository<Certificate>
{
    IEnumerable<Certificate> GetCertificatesWithFilters(string? title, string? userId, string? courseId);
}
