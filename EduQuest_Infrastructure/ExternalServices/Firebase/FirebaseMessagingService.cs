using EduQuest_Application.Abstractions.Firebase;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace EduQuest_Infrastructure.ExternalServices.Firebase
{
	public class FirebaseMessagingService : IFirebaseMessagingService
	{
		
		public async Task<string> SendNotificationAsync(string receiverToken, string senderId, string messageContent)
		{
			var message = new Message
			{
				Token = receiverToken,
				Notification = new Notification
				{
					Title = "New Message",
					Body = $"Message from {senderId}: {messageContent}"
				},
				Data = new Dictionary<string, string>
			{
				{ "senderId", senderId },
				{ "messageContent", messageContent }
			}
			};

			return await FirebaseMessaging.DefaultInstance.SendAsync(message);
		}
	}
}
