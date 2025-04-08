namespace EduQuest_Application.Abstractions.Firebase;

public interface IFireBaseRealtimeService
{
    Task PushNotificationAsync(string userId, string title, string message);
}
