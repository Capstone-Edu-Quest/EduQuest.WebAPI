using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Command.ReviewAssignment;

public class ReviewAssignmentHandler : IRequestHandler<ReviewAssignmentCommand, APIResponse>
{
    private readonly IReviewAssignmentRepository _reviewAssignmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAssignmentAttemptRepository _assignmentAttemptRepository;
    public ReviewAssignmentHandler(IReviewAssignmentRepository reviewAssignmentRepository, IUnitOfWork unitOfWork,
        IAssignmentRepository assignmentRepository, IAssignmentAttemptRepository assignmentAttemptRepository)
    {
        _reviewAssignmentRepository = reviewAssignmentRepository;
        _unitOfWork = unitOfWork;
        _assignmentRepository = assignmentRepository;
        _assignmentAttemptRepository = assignmentAttemptRepository;
    }

    public async Task<APIResponse> Handle(ReviewAssignmentCommand request, CancellationToken cancellationToken)
    {
        var attempt = await _assignmentAttemptRepository.GetById(request.grade.AssignmentAttemptId);
        if (attempt == null)
        {
            return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound,
                MessageCommon.NotFound, "name", $"assignment attempt id: {request.grade.AssignmentAttemptId}");
        }
        if (attempt.UserId == request.UserId)
        {
            return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, MessageCommon.UserDontHavePer,
                MessageCommon.Blocked, "name", $"author cannot grade id: {request.grade.AssignmentAttemptId}");
        }
        AssignmentPeerReview review = new AssignmentPeerReview();
        review.AssignmentAttemptId = request.grade.AssignmentAttemptId;
        review.ReviewerId = request.UserId;
        review.Grade = request.grade.Grade;
        review.Comment = request.grade.Comment;
        int totalReviewer = attempt.Reviewers.Count + 1;
        int totalPoint = attempt.Reviewers.Sum(r => r.Grade) + request.grade.Grade;
        attempt.AnswerScore = Math.Round((double)totalPoint / totalReviewer, 2);
        
        await _assignmentAttemptRepository.Update(attempt);
        await _reviewAssignmentRepository.Add(review);
        await _unitOfWork.SaveChangesAsync();

        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.Complete,
            null, "name", "assignment review");
    }
}
