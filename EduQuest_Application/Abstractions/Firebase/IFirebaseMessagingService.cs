using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.Abstractions.Firebase
{
	public interface IFirebaseMessagingService
	{
		Task<string> SendNotificationAsync(string receiverToken, string senderId, string messageContent);
	}
}
