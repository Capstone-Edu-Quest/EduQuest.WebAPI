using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.UserMetas.Query.GetStatisticForInstructor
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
