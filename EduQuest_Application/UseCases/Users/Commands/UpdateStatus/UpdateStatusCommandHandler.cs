using AutoMapper;
using EduQuest_Application.DTO.Response.Users;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Users.Commands.UpdateStatus;

public class UpdateStatusCommandHandler : IRequestHandler<UpdateStatusCommand, APIResponse>
{
    private readonly IUserRepository _userRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateStatusCommandHandler(IUserRepository userRepo, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userRepo = userRepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetById(request.UserId);
        if (user == null)
        {
            return GeneralHelper.CreateErrorResponse(
                HttpStatusCode.NotFound,
                MessageCommon.NotFound,
                MessageCommon.NotFound,
                "name",
                request.UserId
            );
        }

        user.Status = request.Status;

        await _userRepo.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return GeneralHelper.CreateSuccessResponse(
            HttpStatusCode.OK,
            MessageCommon.UpdateSuccesfully,
            null,
            "user",
            user.Username
        );
    }
}
