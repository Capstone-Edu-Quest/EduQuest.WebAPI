using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Models.Response
{
	public class APIResponse
	{
		public bool IsError { get; set; }
		public object? Payload { get; set; }
		public ErrorResponse? Errors { get; set; }
	}
}
