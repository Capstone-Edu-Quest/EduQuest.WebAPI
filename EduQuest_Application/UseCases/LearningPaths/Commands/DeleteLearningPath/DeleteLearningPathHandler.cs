using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.UseCases.LearningPaths.Commands.DeleteLearningPath;

public class DeleteLearningPathHandler : IRequestHandler<DeleteLearningPathCommand, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLearningPathHandler(ILearningPathRepository learningPathRepository,
        IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _learningPathRepository = learningPathRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<APIResponse> Handle(DeleteLearningPathCommand request, CancellationToken cancellationToken)
    {
        try
        {
            #region validate ownership
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
            {
                return new APIResponse
                {
                    IsError = true,
                    Payload = null,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = MessageCommon.NotFound,
                        StatusResponse = HttpStatusCode.BadRequest
                    },
                    Message = new MessageResponse
                    {
                        content = MessageCommon.NotFound,
                        values = new Dictionary<string, string> { { "name", "user" } }
                    }
                };
            }
            var learningPath = await _learningPathRepository.GetById(request.LearningPathId);
            if (learningPath == null)
            {
                return new APIResponse
                {
                    IsError = true,
                    Payload = null,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = MessageCommon.NotFound,
                        StatusResponse = HttpStatusCode.BadRequest
                    },
                    Message = new MessageResponse
                    {
                        content = MessageCommon.NotFound,
                        values = new Dictionary<string, string> { { "name", "learning path" } }
                    }
                };
            }
            if (learningPath.UserId != user.Id)
            {
                return new APIResponse
                {
                    IsError = true,
                    Payload = null,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = MessageCommon.DeleteFailed,
                        StatusResponse = HttpStatusCode.BadRequest
                    },
                    Message = new MessageResponse
                    {
                        content = MessageCommon.UserDontHavePer,
                        values = new Dictionary<string, string> { { "name", "learning path" } }
                    }
                };
            }
            #endregion
            await _learningPathRepository.Delete(learningPath.Id);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                CommonUserResponse userResponse = _mapper.Map<CommonUserResponse>(learningPath.User);
                MyLearningPathResponse myLearningPathResponse = _mapper.Map<MyLearningPathResponse>(learningPath);
                myLearningPathResponse.TotalCourses = learningPath.LearningPathCourses.Count;
                myLearningPathResponse.CreatedBy = userResponse;
                return new APIResponse
                {
                    IsError = false,
                    Payload = myLearningPathResponse,
                    Errors = null,
                    Message = new MessageResponse
                    {
                        content = MessageCommon.DeleteSuccessfully,
                        values = new Dictionary<string, string> { { "name", "learning path" } }
                    }
                };
            }
            return new APIResponse
            {
                IsError = false,
                Payload = learningPath,
                Errors = null,
                Message = new MessageResponse
                {
                    content = MessageCommon.DeleteFailed,
                    values = new Dictionary<string, string> { { "name", "learning path" } }
                }
            };
        }catch (Exception ex)
        {
            return new APIResponse
            {
                IsError = true,
                Payload = null,
                Errors = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message,
                    StatusResponse = HttpStatusCode.BadRequest
                },
                Message = new MessageResponse
                {
                    content = MessageCommon.DeleteFailed,
                    values = new Dictionary<string, string> { { "name", "learning path" } }
                }
            };
        }
    }
}
