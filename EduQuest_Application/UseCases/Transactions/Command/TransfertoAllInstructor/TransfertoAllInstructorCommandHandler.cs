using EduQuest_Domain.Repository;
using MediatR;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Transactions.Command.TransfertoAllInstructor
{
	public class TransfertoAllInstructorCommandHandler : IRequestHandler<TransfertoAllInstructorCommand, Unit>
	{
		private readonly ITransactionDetailRepository _transactionDetailRepo;
		private readonly IUserRepository _userRepository;

		public TransfertoAllInstructorCommandHandler(ITransactionDetailRepository transactionDetailRepo, IUserRepository userRepository)
		{
			_transactionDetailRepo = transactionDetailRepo;
			_userRepository = userRepository;
		}

		public async Task<Unit> Handle(TransfertoAllInstructorCommand request, CancellationToken cancellationToken)
		{
			var transferList = await _transactionDetailRepo.GetGroupedInstructorTransfersByTransactionId(request.TransactionId);

			foreach (var item in transferList)
			{
				var instructor = await _userRepository.GetById(item.InstructorId);
				if (instructor == null || string.IsNullOrEmpty(instructor.StripeAccountId)) continue;

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
			return Unit.Value;
		}
	}
}
