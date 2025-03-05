using AutoMapper;
using EduQuest_Application.DTO.Response.Certificates;
using EduQuest_Application.UseCases.Certificates.Commands.CreateCertificate;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.Generic;
using EduQuest_Domain.Repository.UnitOfWork;
using FluentAssertions;
using Moq;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Test.CertificateTest;

public class CertificateUnitTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IGenericRepository<Certificate>> _mockCertificateRepo;
    private readonly Mock<ILearnerRepository> _mockLearnerRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateCertificateCommandHandler _handler;

    public CertificateUnitTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockCertificateRepo = new Mock<IGenericRepository<Certificate>>();
        _mockLearnerRepo = new Mock<ILearnerRepository>();
        _mockMapper = new Mock<IMapper>();

        _handler = new CreateCertificateCommandHandler(
            _mockUnitOfWork.Object,
            _mockCertificateRepo.Object,
            _mockLearnerRepo.Object,
            _mockMapper.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateCertificateSuccessfully()
    {
        // Arrange
        var request = new CreateCertificateCommand
        {
            UserId = "user123",
            CourseId = "course456",
            Title = "Certificate of Completion",
            Url = "http://example.com/certificate.pdf"
        };

        var existingLearner = new CourseLearner { UserId = request.UserId, CourseId = request.CourseId };
        var newCertificate = new Certificate
        {
            Title = request.Title,
            Url = request.Url,
            UserId = request.UserId,
            CourseId = request.CourseId
        };

        var certificateDto = new CertificateDto { Title = request.Title, Url = request.Url };

        _mockLearnerRepo.Setup(repo => repo.GetByUserIdAndCourseId(request.UserId, request.CourseId))
                        .ReturnsAsync(existingLearner);

        _mockCertificateRepo.Setup(repo => repo.Add(It.IsAny<Certificate>())).Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _mockMapper.Setup(mapper => mapper.Map<CertificateDto>(It.IsAny<Certificate>())).Returns(certificateDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Payload.Should().NotBeNull();
        result.Payload.Should().BeEquivalentTo(certificateDto);
        result.Message.content.Should().Be(MessageCommon.CreateSuccesfully);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenLearnerNotFound()
    {
        // Arrange
        var request = new CreateCertificateCommand
        {
            UserId = "invalidUser",
            CourseId = "course456",
            Title = "Certificate of Completion",
            Url = "http://example.com/certificate.pdf"
        };

        _mockLearnerRepo.Setup(repo => repo.GetByUserIdAndCourseId(request.UserId, request.CourseId))
                        .ReturnsAsync((CourseLearner)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeNull();
        result.Errors.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        result.Message.content.Should().Be(MessageCommon.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenSavingFails()
    {
        // Arrange
        var request = new CreateCertificateCommand
        {
            UserId = "user123",
            CourseId = "course456",
            Title = "Certificate of Completion",
            Url = "http://example.com/certificate.pdf"
        };

        var existingLearner = new CourseLearner { UserId = request.UserId, CourseId = request.CourseId };
        var newCertificate = new Certificate
        {
            Title = request.Title,
            Url = request.Url,
            UserId = request.UserId,
            CourseId = request.CourseId
        };

        _mockLearnerRepo.Setup(repo => repo.GetByUserIdAndCourseId(request.UserId, request.CourseId))
                        .ReturnsAsync(existingLearner);

        _mockCertificateRepo.Setup(repo => repo.Add(It.IsAny<Certificate>())).Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Errors.Should().BeNull();
        result.Message.content.Should().Be(MessageCommon.CreateFailed);
    }
}
