using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;

namespace EduQuest_Domain.Repository;

public interface IInstructorCertificate : IGenericRepository<InstructorCertificate>
{
    Task BulkCreateAsync(List<InstructorCertificate> certificates);
}

