using EduQuest_Domain.Models.Notification;

namespace EduQuest_Application.Abstractions.Firebase;

public interface IFireBaseRealtimeService
{
    Task PushNotificationAsync(NotificationDto request);
	Task<List<NotificationDto>> GetNotificationsAsync(string receiverId);
}
