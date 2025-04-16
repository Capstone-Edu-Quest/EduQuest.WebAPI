using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.WebStatistics.Queries.GetStatisticForInstructor
{
	public class GetStatisticForInstructorQueryHandler : IRequestHandler<GetStatisticForInstructorQuery, APIResponse>
    {
        private readonly ICourseStatisticRepository _courseStatisticRepository;
        private readonly ICourseRepository _courseRepository;

		public Task<APIResponse> Handle(GetStatisticForInstructorQuery request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}


		//      public async Task<APIResponse> Handle(GetStatisticForInstructorQuery request, CancellationToken cancellationToken)
		//      {
		//          var myCourseIds = (await _courseRepository.GetCourseByUserId(request.UserId)).Select(x => x.Id).Distinct().ToList();
		//          var totalCourses = myCourseIds.Count();
		//          var totalLearners = (await _courseStatisticRepository.GetTotalLearnerForInstructor(myCourseIds)).totalLearner;
		//          var avgReview = (await _courseStatisticRepository.GetTotalLearnerForInstructor(myCourseIds)).avgRating;

		//}
	}
}
