using AutoMapper;
using EduQuest_Application.DTO.Response.Certificates;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.Generic;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Certificates.Commands.CreateCertificate;

public class CreateCertificateCommandHandler : IRequestHandler<CreateCertificateCommand, APIResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Certificate> _certificateRepository;
    private readonly ILearnerRepository _learnerRepository;
    private readonly IMapper _mapper;

    public CreateCertificateCommandHandler(IUnitOfWork unitOfWork, IGenericRepository<Certificate> certificateRepository, ILearnerRepository learnerRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _certificateRepository = certificateRepository;
        _learnerRepository = learnerRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(CreateCertificateCommand request, CancellationToken cancellationToken)
    {
        var existLearner = await _learnerRepository.GetByUserIdAndCourseId(request.UserId, request.CourseId);
        if(existLearner == null)
        {
            return new APIResponse
            {
                IsError = true,
                Errors = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound
                },
                Message = new MessageResponse { content = MessageCommon.NotFound, values = new { name = "certificate" } }
            };
        }

        var newCertificate = new Certificate
        {
            Title = request.Title,
            Url = request.Url,
            
            CourseId = request.CourseId,
        };

        await _certificateRepository.Add(newCertificate);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        var mapObject = _mapper.Map<CertificateDto>(newCertificate);
        return new APIResponse
        {
            IsError = false,
            Errors = null,
            Payload = mapObject,
            Message = new MessageResponse 
            { 
                content = (result>0) ? MessageCommon.CreateSuccesfully : MessageCommon.CreateFailed,
                values = new { name = "certificate" }
            }
        };
    }
}
