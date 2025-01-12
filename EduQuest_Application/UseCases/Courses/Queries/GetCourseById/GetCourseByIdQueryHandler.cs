using EduQuest_Application.UseCases.Courses.Queries.SearchCourse;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Courses.Queries.GetCourseById
{
	public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;

		public Task<APIResponse> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
