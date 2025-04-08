using EduQuest_Application.Abstractions.Firebase;
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

    public async Task PushNotificationAsync(string userId, string title, string message)
    {
        var notification = new
        {
            title = title,
            message = message,
            timestamp = DateTime.UtcNow.ToString("o")
        };


        await _firebaseClient
            .Child("notifications")
            .Child(userId)
            .PostAsync(notification);
    }
}
