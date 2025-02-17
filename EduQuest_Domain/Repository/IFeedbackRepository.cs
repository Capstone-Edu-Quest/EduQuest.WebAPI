using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface IFeedbackRepository : IGenericRepository<Feedback>
{
    Task<PagedList<Feedback>> GetByCourseId(string courseId, int pageNo, int pageSize, int? rating, string? feedback);
    Task<bool> IsOnwer(string feedbackId, string UserId);
}
