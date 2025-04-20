using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Revenue.Query.GetRevenueReportForIntructor
{
	public class GetRevenueReportForIntructorQueryHandler : IRequestHandler<GetRevenueReportForIntructorQuery, APIResponse>
	{
		private readonly ITransactionDetailRepository _transactionDetailRepository;
		private readonly IUserRepository _userRepository;

		public GetRevenueReportForIntructorQueryHandler(ITransactionDetailRepository transactionDetailRepository, IUserRepository userRepository)
		{
			_transactionDetailRepository = transactionDetailRepository;
			_userRepository = userRepository;
		}

		public async Task<APIResponse> Handle(GetRevenueReportForIntructorQuery request, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetById(request.UserId);
			var result = await _transactionDetailRepository.GetRevenueReportAsync(request.UserId, user.Email);
			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, result, "name", $"Revenue of User ID {request.UserId}");
		}

	}
}
