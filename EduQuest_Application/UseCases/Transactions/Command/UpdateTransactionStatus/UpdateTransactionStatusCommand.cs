using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Transactions.Command.UpdateTransactionStatus
{
    public class UpdateTransactionStatusCommand : IRequest<APIResponse>
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
        public UpdateTransactionStatusCommand() { }

		public UpdateTransactionStatusCommand(string transactionId, string status, string userId)
		{
			TransactionId = transactionId;
			Status = status;
			UserId = userId;
		}
	}
}
