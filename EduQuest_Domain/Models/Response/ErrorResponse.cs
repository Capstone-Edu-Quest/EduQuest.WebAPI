using System.Net;
using System.Text.Json;

namespace EduQuest_Domain.Models.Response
{
	public class ErrorResponse
	{
		public int StatusCode { get; set; }
        public HttpStatusCode StatusResponse { get; set; }
        public string? Message { get; set; }
		public string? Location { get; set; }
		public string? Detail { get; set; }

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
