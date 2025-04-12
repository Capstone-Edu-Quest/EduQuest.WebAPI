using MediatR;

namespace EduQuest_Application.UseCases.Transactions.Command.TransfertoAllInstructor
{
	public class TransfertoAllInstructorCommand : IRequest<Unit>
	{
		public string TransactionId { get; set; }

		public TransfertoAllInstructorCommand(string transactionId)
		{
			TransactionId = transactionId;
		}
	}
}
