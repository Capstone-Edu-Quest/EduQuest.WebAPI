using EduQuest_Application.Abstractions.Firebase;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases
{
	public class SendMessageWithNotificationCommandHanlder : IRequestHandler<SendMessageWithNotificationCommand, APIResponse>
	{
		private readonly IFirebaseFirestoreService _firestoreService;
		private readonly IFirebaseMessagingService _firebaseMessagingService;

		public SendMessageWithNotificationCommandHanlder(IFirebaseFirestoreService firestoreService, IFirebaseMessagingService firebaseMessagingService)
		{
			_firestoreService = firestoreService;
			_firebaseMessagingService = firebaseMessagingService;
		}

		public async Task<APIResponse> Handle(SendMessageWithNotificationCommand request, CancellationToken cancellationToken)
		{
			// Create a message object
			var message = new Message
			{
				SenderId = request.Request.SenderId!,
				ReceiverId = request.Request.ReceiverId!,
				Content = request.Request.Message!,
				Timestamp = DateTime.Now
			};

			// Save the message to Firebase Firestore
			await _firestoreService.SaveMessageAsync("messages", message);

			//Get FCM Token from FireStore base on UserId/ReceiverId
			var receiverToken = await _firestoreService.GetUserTokenAsync(request.Request.ReceiverId!);

			if (string.IsNullOrEmpty(receiverToken))
			{
				return new APIResponse
				{
					IsError = true,
					Message = new MessageResponse
					{
						content = MessageCommon.NotFound,
						values = new Dictionary<string, string> { { "name", $"User{request.Request.ReceiverId!}" } }
					}
				};
			}
			string formattedMessage = ContentHelper.ReplacePlaceholders(request.Request.Message!, request.Request.Content);

			// Send Firebase notification
			await _firebaseMessagingService.SendNotificationAsync(
				receiverToken: receiverToken,
				senderId: request.Request.SenderId!,
				messageContent: formattedMessage
			);

			return new APIResponse
			{
				IsError = false,
				Payload = null,
				Errors = null,
				Message = new MessageResponse
				{
					content = request.Request.Message!,
					//values = request.Request.Content
				}
			};

		}
	}
}
