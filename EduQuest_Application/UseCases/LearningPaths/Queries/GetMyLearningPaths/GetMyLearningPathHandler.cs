using AutoMapper;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.LearningPaths.Queries.GetMyLearningPaths;

public class GetMyLearningPathHandler : IRequestHandler<GetMyLearningPathQuery, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IMapper _mapper;
    private const string Key = "name";
    private const string value = "learning path";
    public GetMyLearningPathHandler(ILearningPathRepository learningPathRepository, IMapper mapper)
    {
        _learningPathRepository = learningPathRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetMyLearningPathQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _learningPathRepository.GetMyLearningPaths(request.UserId, request.KeyWord, request.IsPublic, 
                request.IsEnrolled, request.CreatedByExpert, request.Page, request.EachPage);

            var temp = result.Items.ToList();
            List<MyLearningPathResponse> responseDto = new List<MyLearningPathResponse>();
            foreach (var item in temp)
            {
                CommonUserResponse userResponse = _mapper.Map<CommonUserResponse>(item.User);
                MyLearningPathResponse myLearningPathResponse = _mapper.Map<MyLearningPathResponse>(item);
                myLearningPathResponse.TotalCourses = item.LearningPathCourses.Count;
                myLearningPathResponse.CreatedBy = userResponse;
                var enroll = item.Enrollers.Where(e => e.UserId == request.UserId).FirstOrDefault();
                myLearningPathResponse.IsEnrolled = enroll != null;
                responseDto.Add(myLearningPathResponse);
            }
            PagedList<MyLearningPathResponse> response = new PagedList<MyLearningPathResponse>(responseDto, result.TotalItems, result.CurrentPage, result.EachPage);
            //"A84B8BBD-73E0-4857-85A8-C16136C214C8"
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully,
                response, Key, value);

        }catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.GetFailed, ex.Message, Key, value);
        }
    }
}
