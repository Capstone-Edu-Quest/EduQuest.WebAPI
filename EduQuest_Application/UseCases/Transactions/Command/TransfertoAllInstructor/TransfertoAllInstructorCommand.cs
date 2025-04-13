using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Transactions.Command.TransfertoAllInstructor
{
	public class TransfertoAllInstructorCommand : IRequest<APIResponse>
	{
		public string TransactionId { get; set; }
		public TransfertoAllInstructorCommand() { }
		public TransfertoAllInstructorCommand(string transactionId)
		{
			TransactionId = transactionId;
		}
	}
}
