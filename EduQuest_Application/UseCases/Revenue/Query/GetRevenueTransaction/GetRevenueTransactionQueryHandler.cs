using AutoMapper;
using EduQuest_Application.DTO.Response.Revenue;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Revenue.Query.GetRevenueTransaction
{
	public class GetRevenueTransactionQueryHandler : IRequestHandler<GetRevenueTransactionQuery, APIResponse>
	{
		private readonly ITransactionDetailRepository _transactionDetailRepository;
		private readonly ITransactionRepository _transactionRepository;
		private readonly ICourseRepository _courseRepository;
		private readonly IMapper _mapper;

		public GetRevenueTransactionQueryHandler(ITransactionDetailRepository transactionDetailRepository, ITransactionRepository transactionRepository, ICourseRepository courseRepository, IMapper mapper)
		{
			_transactionDetailRepository = transactionDetailRepository;
			_transactionRepository = transactionRepository;
			_courseRepository = courseRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(GetRevenueTransactionQuery request, CancellationToken cancellationToken)
		{
			var listTransactionDetail = await _transactionDetailRepository.GetByInstructorId(request.UserId, request.DateTo, request.DateFrom);
			if(listTransactionDetail is null) return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, listTransactionDetail, "name", "Revenue Transaction");
			var listResponse = (_mapper.Map<List<RevenueTransaction>>(listTransactionDetail)).OrderByDescending(x => x.UpdatedAt);
			foreach (var transactionDetail in listResponse)
			{
				var course = await _courseRepository.GetById(transactionDetail.ItemId);
				transactionDetail.Title = course.Title;

				var listTransfer = await _transactionRepository.CheckTransfer(transactionDetail.TransactionId);
				if (listTransfer.Any())
				{
					transactionDetail.IsReceive = true;
					transactionDetail.ReceiveDate = listTransfer.Where(x => x.UpdatedAt != null).FirstOrDefault().UpdatedAt;
				}
				else
				{
					transactionDetail.IsReceive = false;
				}
			}
			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, listResponse, "name", "Revenue Transaction");


		}
	}
}
