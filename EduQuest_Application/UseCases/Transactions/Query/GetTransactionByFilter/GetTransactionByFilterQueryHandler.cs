using AutoMapper;
using EduQuest_Application.DTO.Response.Transactions;
using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;

namespace EduQuest_Application.UseCases.Transactions.Query.GetTransactionByFilter;

public class GetTransactionByFilterQueryHandler : IRequestHandler<GetTransactionByFilterQuery, APIResponse>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;

    public GetTransactionByFilterQueryHandler(ITransactionRepository transactionRepository, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetTransactionByFilterQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _transactionRepository.GetTransactionByFilter(request.TransactionId, request.UserId, request.Status, request.Type, request.CourseId);
        var mappedResult = _mapper.Map<List<TransactionDto>>(transactions);
        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, Constants.MessageCommon.GetSuccesfully, mappedResult, "name", "");


    }
}
