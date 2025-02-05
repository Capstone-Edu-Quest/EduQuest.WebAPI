using EduQuest_Application.Abstractions.Firebase;
using EduQuest_Application.DTO.Response;
using EduQuest_Domain.Models.Response;
using MediatR;

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
				SenderId = request.Request.SenderId,
				ReceiverId = request.Request.ReceiverId,
				Content = request.Request.Content,
				Timestamp = DateTime.UtcNow
			};

			// Save the message to Firebase Firestore
			await _firestoreService.SaveMessageAsync("messages", message);

			// Send Firebase notification
			await _firebaseMessagingService.SendNotificationAsync(
				receiverToken: request.Request.ReceiverToken,
				senderId: request.Request.SenderId,
				messageContent: request.Request.Content
			);

			return new APIResponse
			{
				IsError = false,
				Payload = message,
				Errors = null
			};

		}
	}
}
