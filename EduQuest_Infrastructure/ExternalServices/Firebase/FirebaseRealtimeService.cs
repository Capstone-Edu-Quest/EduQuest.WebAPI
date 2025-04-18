using EduQuest_Application.Abstractions.Firebase;
using EduQuest_Domain.Models.Notification;
using Firebase.Database;
using Firebase.Database.Query;

namespace EduQuest_Infrastructure.ExternalServices.Firebase;

public class FirebaseRealtimeService : IFireBaseRealtimeService
{
    private readonly FirebaseClient _firebaseClient;

    public FirebaseRealtimeService(string firebaseDbUrl)
    {
        _firebaseClient = new FirebaseClient(firebaseDbUrl);
    }

	public async Task<List<NotificationDto>> GetNotificationsAsync(string receiverId)
	{
		var notifications = await _firebaseClient
		.Child("notifications")
		.Child(receiverId)
		.OnceAsync<NotificationDto>();

		return notifications
			.Select(n => new NotificationDto
			{
				Content = n.Object.Content,
				Receiver = n.Object.Receiver,
				Url = n.Object.Url,
				userId = receiverId,
				Values = n.Object.Values
			})
			.ToList();
	}

	public async Task PushNotificationAsync(NotificationDto request)
    {
        string generatedId = Guid.NewGuid().ToString();
        var notification = new
        {
            content = request.Content,
            id = generatedId,
            receiverId = request.Receiver,
            url = request.Url,
            timestamp = DateTime.UtcNow.ToString("o"),
            value = request.Values
        };


        await _firebaseClient
            .Child("notifications")
            .Child(request.userId)
            .Child(generatedId)
            .PutAsync(notification);
    }
}
