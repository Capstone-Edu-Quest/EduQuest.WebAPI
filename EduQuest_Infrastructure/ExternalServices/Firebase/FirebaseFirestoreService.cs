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

		public async Task<DocumentReference> SaveMessageAsync(string collection, object data)
		{
			var docRef = await _firestore.Collection(collection).AddAsync(data);
			return docRef;
		}
	}
}
