using Google.Cloud.Firestore;

namespace EduQuest_Application.Abstractions.Firebase
{
	public interface IFirebaseFirestoreService
	{
		Task<DocumentReference> SaveMessageAsync(string collection, object data);
		Task<QuerySnapshot> GetMessagesAsync(string collection, string receiverId);
		Task DeleteMessageAsync(string collection, string documentId);
		Task<string?> GetUserTokenAsync(string userId);
		Task SaveUserTokenAsync(string userId, string token);
	}
}
