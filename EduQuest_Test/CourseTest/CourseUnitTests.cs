using AutoMapper;
using EduQuest_Application.UseCases.Courses.Command.CreateCourse;
using EduQuest_Application.UseCases.Courses.Command.UpdateCourse;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using FluentAssertions;
using Moq;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Test.CourseTest;
public class CourseUnitTests
{
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IStageRepository> _stageRepositoryMock;
    private readonly Mock<IUserStatisticRepository> _userStatisticsMock;
    private readonly Mock<ILearningMaterialRepository> _learningMaterialRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUserStatisticRepository> _userStatisticRepositoryMock;
    private readonly CreateCourseCommandHandler _handler;
    private readonly UpdateCourseCommandHandler _updateCourseHandler;

    public CourseUnitTests()
    {
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userStatisticsMock = new Mock<IUserStatisticRepository>();
        _stageRepositoryMock = new Mock<IStageRepository>();
        _learningMaterialRepositoryMock = new Mock<ILearningMaterialRepository>();
        _mapperMock = new Mock<IMapper>();
        _userStatisticRepositoryMock = new Mock<IUserStatisticRepository>();

        _handler = new CreateCourseCommandHandler(
            _courseRepositoryMock.Object,
            _mapperMock.Object,
            _unitOfWorkMock.Object,
            _userRepositoryMock.Object,
            _userStatisticsMock.Object
        );

        _updateCourseHandler = new UpdateCourseCommandHandler(
            _courseRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _stageRepositoryMock.Object,
            _learningMaterialRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateCourseSuccessfully()
    {
        // Arrange
        var request = new CreateCourseCommand(new()
        {
            Title = "Test Course",
            Description = "Test Description"
        }, "user-123");

        var user = new User { Id = "user-123", Username = "Test User" };
        var course = new Course { Title = "Test Course", CreatedBy = user.Id };

        _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<string>()))
            .ReturnsAsync(user);

        _mapperMock.Setup(mapper => mapper.Map<Course>(request.CourseRequest))
            .Returns(course);

        _courseRepositoryMock.Setup(repo => repo.Add(It.IsAny<Course>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Payload.Should().BeOfType<Course>();
        result.Message.content.Should().Be(MessageCommon.CreateSuccesfully);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserNotFound()
    {
        // Arrange
        var request = new CreateCourseCommand(new()
        {
            Title = "Test Course"
        }, "invalid-user");

        _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<string>()))
            .ReturnsAsync((User)null); 

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.Errors.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        result.Message.content.Should().Be(MessageCommon.CreateFailed);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenSaveFails()
    {
        // Arrange
        var request = new CreateCourseCommand(new()
        {
            Title = "Test Course"
        }, "user-123");

        var user = new User { Id = "user-123", Username = "Test User" };
        var course = new Course { Title = "Test Course", CreatedBy = user.Id };

        _userRepositoryMock.Setup(repo => repo.GetById(It.IsAny<string>()))
            .ReturnsAsync(user);

        _mapperMock.Setup(mapper => mapper.Map<Course>(request.CourseRequest))
            .Returns(course);

        _courseRepositoryMock.Setup(repo => repo.Add(It.IsAny<Course>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.Errors.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        result.Message.content.Should().Be(MessageCommon.CreateFailed);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenCourseNotFound()
    {
        // Arrange
        var request = new UpdateCourseCommand(new()
        {
            CourseId = "invalid-course-id"
        });

        _courseRepositoryMock.Setup(repo => repo.GetById(It.IsAny<string>()))
            .ReturnsAsync((Course)null);

        // Act
        var result = await _updateCourseHandler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.Errors.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        result.Message.content.Should().Be(MessageCommon.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldUpdateCourseSuccessfully()
    {
        // Arrange
        var request = new UpdateCourseCommand(new()
        {
            CourseId = "course-123",
            Title = "Updated Course Title",
            Description = "Updated Description"
        });

        var existingCourse = new Course
        {
            Id = "course-123",
            Title = "Old Title",
            Description = "Old Description"
        };

        _courseRepositoryMock.Setup(repo => repo.GetById(It.IsAny<string>()))
            .ReturnsAsync(existingCourse);

        _courseRepositoryMock.Setup(repo => repo.Update(It.IsAny<Course>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1); 

        // Act
        var result = await _updateCourseHandler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Message.content.Should().Be(MessageCommon.UpdateSuccesfully); 
    }


}
