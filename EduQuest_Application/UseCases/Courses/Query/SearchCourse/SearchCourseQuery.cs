using EduQuest_Application.DTO.Request.Courses;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Courses.Queries.SearchCourse
{
    public class SearchCourseQuery : IRequest<APIResponse>
	{
		public int PageNo { get; set; }
		public int EachPage { get; set; }
        public SearchCourseRequest SearchRequest { get; set; }

		public SearchCourseQuery(int pageNo, int eachPage, SearchCourseRequest searchRequest)
		{
			PageNo = pageNo;
			EachPage = eachPage;
			SearchRequest = searchRequest;
		}
	}
}
