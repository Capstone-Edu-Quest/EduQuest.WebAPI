using AutoMapper;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.LearningPaths.Queries.GetByCourseId;

public class GetByCourseIdHandler : IRequestHandler<GetByCourseIdQuery, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IMapper _mapper;

    public GetByCourseIdHandler(ILearningPathRepository learningPathRepository, IMapper mapper)
    {
        _learningPathRepository = learningPathRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetByCourseIdQuery request, CancellationToken cancellationToken)
    {
        List<LearningPathPreview> responses = new List<LearningPathPreview>();
        var learningPaths = await _learningPathRepository.GetByCourseId(request.CourseId, request.UserId);
        if (learningPaths == null)
        {
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully,
                responses, "name", "learning path");
        }
        foreach(var item in learningPaths)
        {
            LearningPathPreview temp = _mapper.Map<LearningPathPreview>(item);
            var enroller = item.Enrollers.Where(e => e.IsOverDue == true && e.CourseId == request.CourseId).FirstOrDefault();
            var enrollerDate = item.Enrollers.Where(e => e.IsOverDue == false && e.CourseId == request.CourseId).FirstOrDefault();
            if (enrollerDate != null)
            {
                temp.EnrollDate = enrollerDate.EnrollDate;
                temp.DueDate = enrollerDate.DueDate;
                temp.IsCompleted = enrollerDate.IsCompleted;
            }
            temp.IsOverDue = enroller != null;
            temp.CreatedBy = _mapper.Map<CommonUserResponse>(item.User);
            responses.Add(temp);
        }
        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully,
                responses, "name", "learning path");
    }
}
