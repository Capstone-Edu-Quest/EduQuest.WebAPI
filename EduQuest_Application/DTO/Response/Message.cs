using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response
{
	[FirestoreData]
	public class Message
	{
		[FirestoreProperty]
		public string SenderId { get; set; }

		[FirestoreProperty]
		public string ReceiverId { get; set; }

		[FirestoreProperty]
		public string Content { get; set; }

		[FirestoreProperty]
		public DateTime Timestamp { get; set; } = DateTime.Now;
	}
}
