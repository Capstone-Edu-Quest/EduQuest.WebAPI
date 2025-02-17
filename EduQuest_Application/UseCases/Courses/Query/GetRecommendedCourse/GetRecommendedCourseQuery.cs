using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Courses.Query.GetRecommendedCourse
{
	public class GetRecommendedCourseQuery : IRequest<APIResponse>
	{
        public string UserId { get; set; }
		public int PageNo { get; set; }
		public int EachPage { get; set; }

		public GetRecommendedCourseQuery(string userId, int pageNo, int eachPage)
		{
			UserId = userId;
			PageNo = pageNo;
			EachPage = eachPage;
		}
	}
}
