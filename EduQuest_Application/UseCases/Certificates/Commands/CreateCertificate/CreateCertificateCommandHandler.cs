using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.Generic;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;

namespace EduQuest_Application.UseCases.Certificates.Commands.CreateCertificate;

public class CreateCertificateCommandHandler : IRequestHandler<CreateCertificateCommand, APIResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Certificate> _certificateRepository;

    public CreateCertificateCommandHandler(IUnitOfWork unitOfWork, IGenericRepository<Certificate> certificateRepository)
    {
        _unitOfWork = unitOfWork;
        _certificateRepository = certificateRepository;
    }

    public Task<APIResponse> Handle(CreateCertificateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
