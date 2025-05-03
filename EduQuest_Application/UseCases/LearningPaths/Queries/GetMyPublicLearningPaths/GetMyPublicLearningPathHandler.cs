using AutoMapper;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.LearningPaths.Queries.GetMyPublicLearningPaths;

public class GetMyPublicLearningPathHandler : IRequestHandler<GetMyPublicLearningPathQuery, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IMapper _mapper;
    private const string Key = "name";
    private const string value = "learning path";
    public GetMyPublicLearningPathHandler(ILearningPathRepository learningPathRepository, IMapper mapper)
    {
        _learningPathRepository = learningPathRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetMyPublicLearningPathQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _learningPathRepository.GetMyPublicLearningPaths(request.UserId, request.KeyWord);
            List<MyPublicLearningPathResponse> responseDto = new List<MyPublicLearningPathResponse>();
            foreach (var item in result)
            {
                CommonUserResponse userResponse = _mapper.Map<CommonUserResponse>(item.User);
                MyPublicLearningPathResponse myLearningPathResponse = _mapper.Map<MyPublicLearningPathResponse>(item);
                myLearningPathResponse.CreatedBy = userResponse;
                myLearningPathResponse.TotalCourses = item.LearningPathCourses.Count;
                if(request.CurrentUserId != null)
                {
                    var enroll = item.Enrollers.Where(l => l.UserId == request.CurrentUserId).FirstOrDefault();    
                    myLearningPathResponse.IsEnrolled = enroll != null;
                    myLearningPathResponse.EnrollDate = enroll != null ? enroll.EnrollDate : null;
                    myLearningPathResponse.TotalEnroller = item.Enrollers.DistinctBy(e => e.UserId).Count();
                }
                else
                {
                    myLearningPathResponse.IsEnrolled = false;
                    myLearningPathResponse.EnrollDate = null;
                }
                responseDto.Add(myLearningPathResponse);
            }
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK,MessageCommon.GetSuccesfully,
                responseDto, Key, value);
        }catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.GetFailed, ex.Message, Key, value);
        }
    }
}
