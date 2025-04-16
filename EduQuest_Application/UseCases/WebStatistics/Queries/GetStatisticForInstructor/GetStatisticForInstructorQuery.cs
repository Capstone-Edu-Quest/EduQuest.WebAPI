using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.WebStatistics.Queries.GetStatisticForInstructor
{
	public class GetStatisticForInstructorQuery : IRequest<APIResponse>
    {
        public string UserId { get; set; }

        public GetStatisticForInstructorQuery(string userId)
        {
            UserId = userId;
        }
    }
}
