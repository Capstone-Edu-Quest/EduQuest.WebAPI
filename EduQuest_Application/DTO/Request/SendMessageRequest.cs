﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Request
{
	public class SendMessageRequest
	{
		public string? SenderId { get; set; }      
		public string? ReceiverId { get; set; }
		//public string? Url { get; set; }
        public string? Message { get; set; }
        public Dictionary<string, string> Content { get; set; }       
		
		
	}
}
