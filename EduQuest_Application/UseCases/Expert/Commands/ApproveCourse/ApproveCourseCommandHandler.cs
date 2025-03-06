using AutoMapper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.Generic;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using System.Reflection.Metadata;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Expert.Commands.ApproveCourse;

public class ApproveCourseCommandHandler : IRequestHandler<ApproveCourseCommand, APIResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Course> _courseRepository;
    private readonly IMapper _mapper;

    public ApproveCourseCommandHandler(IUnitOfWork unitOfWork, IGenericRepository<Course> courseRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(ApproveCourseCommand request, CancellationToken cancellationToken)
    {
        var existingCourse = await _courseRepository.GetById(request.CourseId);
        if (existingCourse == null)
        {
            return new APIResponse
            {
                IsError = true,
                Errors = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound
                },
                Message = new MessageResponse
                {
                    content = MessageCommon.NotFound,
                    values = new { name = "course" }
                }
            };
        }

        if (existingCourse.Status != GeneralEnums.StatusCourse.Pending.ToString())
        {
            return new APIResponse
            {
                IsError = true,
                Errors = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = MessageCommon.CourseShouldBePending
                },
                Message = new MessageResponse
                {
                    content = MessageCommon.CourseShouldBePending
                }
            };
        }

        existingCourse.Status = request.isApprove 
            ? GeneralEnums.StatusCourse.Public.ToString()
            : GeneralEnums.StatusCourse.Draft.ToString();

        await _courseRepository.Update(existingCourse);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new APIResponse
        {
            IsError = false,
            Errors = null,
            Message = new MessageResponse
            {
                content = result > 0 ? MessageCommon.UpdateSuccesfully : MessageCommon.UpdateFailed,
                values = new { name = "course" }
            }
        };
    }
}
