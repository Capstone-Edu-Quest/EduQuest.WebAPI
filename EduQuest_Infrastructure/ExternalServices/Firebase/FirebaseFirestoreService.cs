using EduQuest_Application.Abstractions.Firebase;
using Google.Cloud.Firestore;

namespace EduQuest_Infrastructure.ExternalServices.Firebase
{
	public class FirebaseFirestoreService : IFirebaseFirestoreService
	{
		private readonly FirestoreDb _firestore;

		public FirebaseFirestoreService(FirestoreDb firestore)
		{
			_firestore = firestore;
		}

		public async Task DeleteMessageAsync(string collection, string documentId)
		{
			var docRef = _firestore.Collection(collection).Document(documentId);
			await docRef.DeleteAsync();
		}

		public async Task<QuerySnapshot> GetMessagesAsync(string collection, string receiverId)
		{
			var query = _firestore.Collection(collection).WhereEqualTo("ReceiverId", receiverId);
			return await query.GetSnapshotAsync();
		}

		public async Task<string?> GetUserTokenAsync(string userId)
		{
			var docRef = _firestore.Collection("userTokens").Document(userId);
			var snapshot = await docRef.GetSnapshotAsync();

			if (snapshot.Exists && snapshot.ContainsField("Token"))
			{
				return snapshot.GetValue<string>("Token");
			}
			return null;
		}

		public async Task<DocumentReference> SaveMessageAsync(string collection, object data)
		{
			var docRef = await _firestore.Collection(collection).AddAsync(data);
			return docRef;
		}

		public async Task SaveUserTokenAsync(string userId, string token)
		{
			var docRef = _firestore.Collection("userTokens").Document(userId);
			await docRef.SetAsync(new { Token = token }, SetOptions.MergeAll);
		}
	}
}
