using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;

namespace EduQuest_Application.UseCases.Payments.Command.CancelPayment
{
	public class CancelPaymentCommandHandler : IRequestHandler<CancelPaymentCommand, APIResponse>
	{
		private readonly ITransactionRepository _transactionRepository;
		private readonly ITransactionDetailRepository _transactionDetailRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CancelPaymentCommandHandler(ITransactionRepository transactionRepository, ITransactionDetailRepository transactionDetailRepository, IUnitOfWork unitOfWork)
		{
			_transactionRepository = transactionRepository;
			_transactionDetailRepository = transactionDetailRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<APIResponse> Handle(CancelPaymentCommand request, CancellationToken cancellationToken)
		{
			var transactionPending = await _transactionRepository.CheckTransactionPending(request.UserId);
			transactionPending.Url = null;
			transactionPending.Status = GeneralEnums.StatusPayment.Canceled.ToString();

			await _transactionRepository.Update(transactionPending);

			if (await _unitOfWork.SaveChangesAsync() > 0)
			{
				return GeneralHelper.CreateSuccessResponse(
					HttpStatusCode.OK,
					MessageCommon.UpdateSuccesfully,
					transactionPending,
					"name",
					"Transaction Pending"
				);
			}

			return GeneralHelper.CreateErrorResponse(
				HttpStatusCode.BadRequest,
				MessageCommon.UpdateFailed,
				"Saving Failed",
				"name",
				"Transaction Pending"
			);

		}
	}
}
