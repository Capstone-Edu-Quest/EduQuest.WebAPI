using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using StackExchange.Redis;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.UseCases.Transactions.Command.UpdateTransactionStatus
{
    public class UpdateTransactionStatusCommandHandler : IRequestHandler<UpdateTransactionStatusCommand, APIResponse>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTransactionStatusCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> Handle(UpdateTransactionStatusCommand request, CancellationToken cancellationToken)

        {
            var transactionExisted = await _transactionRepository.GetByPaymentIntentId(request.TransactionId);
            if (transactionExisted == null)
            {
                return new APIResponse
                {
                    IsError = true,
                    Payload = null,
                    Errors = new ErrorResponse
                    {
                        StatusResponse = HttpStatusCode.NotFound,
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Message = MessageCommon.NotFound,
                    },
                    Message = new MessageResponse
                    {
                        content = MessageCommon.NotFound,
                        values = new Dictionary<string, string> { { "name", $"transaction ID {request.TransactionId}" } }
                    }
                };
            }
            transactionExisted.Status = request.Status;
            await _transactionRepository.Update(transactionExisted);

            var result = await _unitOfWork.SaveChangesAsync() > 0;

            return new APIResponse
            {
                IsError = !result,
                Payload = result ? transactionExisted : null,
                Errors = result ? null : new ErrorResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = MessageCommon.SavingFailed,
                },
                Message = new MessageResponse
                {
                    content = result ? MessageCommon.CreateSuccesfully : MessageCommon.CreateFailed,
                    values = new Dictionary<string, string> { { "name", "transaction" } }
                }
            };
        }
    }
}
