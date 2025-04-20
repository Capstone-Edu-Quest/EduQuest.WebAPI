using EduQuest_Application.Abstractions.Firebase;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Notification;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using Stripe;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Transactions.Command.TransfertoAllInstructor
{
	public class TransfertoAllInstructorCommandHandler : IRequestHandler<TransfertoAllInstructorCommand, APIResponse>
	{
		private readonly ITransactionDetailRepository _transactionDetailRepo;
		private readonly ITransactionRepository _transactionRepo;
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;

		public TransfertoAllInstructorCommandHandler(ITransactionDetailRepository transactionDetailRepo, ITransactionRepository transactionRepo, IUserRepository userRepository, IUnitOfWork unitOfWork)
		{
			_transactionDetailRepo = transactionDetailRepo;
			_transactionRepo = transactionRepo;
			_userRepository = userRepository;
			_unitOfWork = unitOfWork;
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
				var newTransaction = new Transaction
				{
					Id = Guid.NewGuid().ToString(),
					TotalAmount = item.TotalInstructorShare,
					Status = GeneralEnums.StatusPayment.Completed.ToString(),
					Type = GeneralEnums.TypeTransaction.Transfer.ToString(),
					UserId = "3b7d5b9c-c7bf-494b-9a75-e931d4a5cb22",
					CustomerEmail = instructor.Email,
					CustomerName = instructor.Username
				};

                //await _firebaseRealtimeService.PushNotificationAsync(
                //new NotificationDto
                //{
                //    userId = transactionExisted.UserId,
                //    Content = NotificationMessage.BUY_COURSE_SUCCESSFULLY,
                //    Receiver = transactionExisted.UserId,
                //    Url = "",
                //    Values = new Dictionary<string, string>
                //    {
                //        { "item", string.Join(",", purchasedCourseNames)}
                //    }
                //}

                await _transactionRepo.Add(newTransaction);
				await _unitOfWork.SaveChangesAsync();

			}
			return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.CreateSuccesfully, null, "name", "Transfer"); ;
		}
	}
}
