using AutoMapper;
using EduQuest_Application.DTO.Response.Profiles;
using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Users.Queries.GetInstructorApplication;

public class CancelApplyInstructorHandler : IRequestHandler<CancelApplyInstructor, APIResponse>
{
    private readonly IUserRepository _userRepo;
    private readonly IInstructorCertificate _certificateRepo;
    private readonly IUserTagRepository _userTagRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CancelApplyInstructorHandler(IUserRepository userRepo, IInstructorCertificate certificateRepo, IUserTagRepository userTagRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userRepo = userRepo;
        _certificateRepo = certificateRepo;
        _userTagRepository = userTagRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(CancelApplyInstructor request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetById(request.userId);
        if (user == null)
        {
            return GeneralHelper.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                MessageCommon.NotFound,
                MessageCommon.NotFound,
                "userId",
                request.userId);
        }

        if (request.isCanceled)
        {
            user.Status = "Cancelled";
            await _userRepo.Update(user);

            await _userTagRepository.DeleteByUserIdAsync(user.Id);

            var userCertificates = await _certificateRepo.GetByUserIdAsync(user.Id);
            if (userCertificates != null && userCertificates.Any())
            {
                foreach (var cert in userCertificates)
                {
                    await _certificateRepo.Delete(cert.Id);
                }
            }
        }

        await _unitOfWork.SaveChangesAsync();
        var mappedResult = _mapper.Map<InstructorProfileDto>(user);
        return GeneralHelper.CreateSuccessResponse(
            HttpStatusCode.OK,
            MessageCommon.CancelSuccessfully,
            mappedResult,
            "user",
            "certificates"
        );
    }
}
