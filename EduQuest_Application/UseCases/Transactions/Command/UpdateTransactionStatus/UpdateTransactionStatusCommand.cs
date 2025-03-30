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
        public string PaymentIntentId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public UpdateTransactionStatusCommand() { }

		public UpdateTransactionStatusCommand(string transactionId, string status, string paymentIntentId, string customerEmail, string customerName)
		{
			TransactionId = transactionId;
			Status = status;
			PaymentIntentId = paymentIntentId;
			CustomerEmail = customerEmail;
			CustomerName = customerName;
		}
	}
}
