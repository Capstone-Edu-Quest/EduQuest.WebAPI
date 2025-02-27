using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface ICertificateRepository : IGenericRepository<Certificate>
{
    PagedList<Certificate> GetCertificatesWithFilters(string? title, string? userId, string? courseId, int? page, int? eachPage);
}
