using AutoMapper;
using EduQuest_Application.DTO.Response.Revenue;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Revenue.Query.GetDetailRevenueTransaction
{
	public class GetDetailRevenueTransactionQueryHandler : IRequestHandler<GetDetailRevenueTransactionQuery, APIResponse>
	{
		private readonly ITransactionDetailRepository _transactionDetailRepository;
		private readonly ITransactionRepository _transactionRepository;
		private readonly IMapper _mapper;

		public GetDetailRevenueTransactionQueryHandler(ITransactionDetailRepository transactionDetailRepository, ITransactionRepository transactionRepository, IMapper mapper
			)
		{
			_transactionDetailRepository = transactionDetailRepository;
			_transactionRepository = transactionRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(GetDetailRevenueTransactionQuery request, CancellationToken cancellationToken)
		{
			var transactionDetail = await _transactionDetailRepository.GetById(request.Id);
			var response = _mapper.Map< DetailRevenueTransaction >(transactionDetail);

			var listTransfer = await _transactionRepository.CheckTransfer(transactionDetail.TransactionId);
			if(listTransfer.Any())
			{
				response.ReceiveDate = listTransfer.Where(x => x.UpdatedAt != null).FirstOrDefault().UpdatedAt;
				response.TransferId = listTransfer.Where(x => x.UpdatedAt != null).FirstOrDefault().Id;
			}

			var transaction = await _transactionRepository.GetById(transactionDetail.TransactionId);
			if (transaction != null)
			{
				response.UserName = transaction.User.Username;
			}
			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, response, "name", "Detail Revenue Transaction");
		}
	}
}
