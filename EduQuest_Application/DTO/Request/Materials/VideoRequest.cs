using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request.Materials
{
	public class VideoRequest
	{
		public string UrlMaterial { get; set; }
		public double? Duration { get; set; }
		public string? Thumbnail { get; set; }
	}
}
