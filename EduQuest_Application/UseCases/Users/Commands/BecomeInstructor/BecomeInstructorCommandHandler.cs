using AutoMapper;
using Azure.Storage.Blobs.Models;
using EduQuest_Application.Abstractions.AzureBlobStorage;
using EduQuest_Application.DTO.Response.Profiles;
using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;

namespace EduQuest_Application.UseCases.Users.Commands.BecomeInstructor;

public class BecomeInstructorCommandHandler : IRequestHandler<BecomeInstructorCommand, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IAzureBlobStorage _azureBlobStorage;
    private readonly IInstructorCertificate _instructorCertificate;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public BecomeInstructorCommandHandler(IUserRepository userRepository, IAzureBlobStorage azureBlobStorage, IInstructorCertificate instructorCertificate, ICourseRepository courseRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _azureBlobStorage = azureBlobStorage;
        _instructorCertificate = instructorCertificate;
        _courseRepository = courseRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(BecomeInstructorCommand request, CancellationToken cancellationToken)
    {
        var existUser = await _userRepository.GetById(request.UserId);
        if (existUser == null)
        {
            return GeneralHelper.CreateErrorResponse(
            HttpStatusCode.NotFound,
            Constants.MessageCommon.NotFound,
            Constants.MessageCommon.NotFound,
            "name",
            "user"
        );
        }

        //Insert certificate in database
        var instructorCertificates = new List<InstructorCertificate>();

        foreach (var file in request.CertificateFiles)
        {
            string originalFileName = file.FileName;
            string uniqueFileName = $"{Guid.NewGuid()}_{originalFileName}";

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;

            var httpHeaders = new BlobHttpHeaders
            {
                ContentType = file.ContentType
            };

            await _azureBlobStorage.UploadAsync(uniqueFileName, memoryStream, httpHeaders);
            string fileUrl = _azureBlobStorage.GetFileUrl(uniqueFileName);

            var certificate = new InstructorCertificate
            {
                Id = Guid.NewGuid().ToString(),
                UserId = request.UserId,
                CertificateUrl = fileUrl,
                CreatedAt = DateTime.UtcNow.ToUniversalTime(),
                UpdatedAt = DateTime.UtcNow.ToUniversalTime()
            };
            instructorCertificates.Add(certificate);
        }
        await _instructorCertificate.BulkCreateAsync(instructorCertificates);

        existUser.Headline = request.Headline;
        existUser.Phone = request.Phone;
        existUser.Description = request.Description;
        existUser.Status = AccountStatus.Pending.ToString();
        await _userRepository.Update(existUser);
        await _unitOfWork.SaveChangesAsync();


        //Getting mapped profile
        var courses = await _courseRepository.GetCoursesByInstructorIdAsync(existUser.Id);
        var courseDtos = _mapper.Map<List<CourseProfileDto>>(courses);

        foreach (var coursedto in courseDtos)
        {
            coursedto.Author = existUser.Username;
        }

        int totalLearners = courses.Sum(c => c.CourseLearners?.Count ?? 0);
        int totalReviews = courses.Sum(c => c.Feedbacks?.Count ?? 0);

        var instructorDto = _mapper.Map<InstructorProfileDto>(existUser);
        instructorDto.Courses = courseDtos;
        instructorDto.TotalLearners = totalLearners;
        instructorDto.TotalReviews = totalReviews;


        return GeneralHelper.CreateSuccessResponse(
            HttpStatusCode.OK,
            Constants.MessageCommon.SubmitBecomeInstructorSuccessfully,
            instructorDto,
            "name",
            existUser.Username
        );
    }
}
