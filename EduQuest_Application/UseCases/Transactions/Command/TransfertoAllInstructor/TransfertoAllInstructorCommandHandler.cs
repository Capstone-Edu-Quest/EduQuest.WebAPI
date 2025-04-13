using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using Stripe;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Transactions.Command.TransfertoAllInstructor
{
	public class TransfertoAllInstructorCommandHandler : IRequestHandler<TransfertoAllInstructorCommand, APIResponse>
	{
		private readonly ITransactionDetailRepository _transactionDetailRepo;
		private readonly IUserRepository _userRepository;

		public TransfertoAllInstructorCommandHandler(ITransactionDetailRepository transactionDetailRepo, IUserRepository userRepository)
		{
			_transactionDetailRepo = transactionDetailRepo;
			_userRepository = userRepository;
		}

		public async Task<APIResponse> Handle(TransfertoAllInstructorCommand request, CancellationToken cancellationToken)
		{
			var transferList = await _transactionDetailRepo.GetGroupedInstructorTransfersByTransactionId(request.TransactionId);

			foreach (var item in transferList)
			{
				var instructor = await _userRepository.GetById(item.InstructorId);
				if (instructor == null || string.IsNullOrEmpty(instructor.StripeAccountId)) return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", $"StripeAccountId Of Intructor ID {item.InstructorId}");

				var options = new TransferCreateOptions
				{
					Amount = (long)(item.TotalInstructorShare * 100), // cents
					Currency = "usd",
					Destination = instructor.StripeAccountId,
					TransferGroup = item.TransferGroup
				};

				var service = new TransferService();
				await service.CreateAsync(options, cancellationToken: cancellationToken);
			}
			return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.CreateSuccesfully, null, "name", "Transfer"); ;
		}
	}
}
