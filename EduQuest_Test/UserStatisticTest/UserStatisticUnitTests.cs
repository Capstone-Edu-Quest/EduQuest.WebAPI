using EduQuest_Application.UseCases.UserMetas.Commands.UpdateUsersStreak;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;
using EduQuest_Domain.Repository.UnitOfWork;
using FluentAssertions;
using Moq;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Test.UserStatisticTest;

public sealed class UserStatisticUnitTests
{
    private readonly UpdateUsersStreakCommandHandler _handler;
    private readonly Mock<IGenericRepository<UserMeta>> _mockRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public UserStatisticUnitTests()
    {
        _mockRepository = new Mock<IGenericRepository<UserMeta>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new UpdateUsersStreakCommandHandler((EduQuest_Domain.Repository.IUserMetaRepository)_mockRepository.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldReturnNotFoundResponse()
    {
        // Arrange
        var invalidId = "test";
        var command = new UpdateUsersStreakCommand { UserId = "test" };
        _mockRepository.Setup(repo => repo.GetById(It.IsAny<string>())).ReturnsAsync((UserMeta)null);


        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.IsError.Should().BeTrue();
        response.Errors.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Handle_LastLearningDayNull_ShouldInitializeIt()
    {
        // Arrange
        var userStat = new UserMeta { Id = "user1", LastLearningDay = null, CurrentStreak = 0, LongestStreak = 0 };
        var command = new UpdateUsersStreakCommand { UserId = "user1" };

        _mockRepository.Setup(repo => repo.GetById(command.UserId)).ReturnsAsync(userStat);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        userStat.LastLearningDay.Should().NotBeNull();
        response.IsError.Should().BeFalse();
        response.Message.content.Should().Be(MessageCommon.UpdateSuccesfully);
    }

    [Fact]
    public async Task Handle_CStreakNotHigherThanLStreak_ShouldBeUnchanged()
    {
        // Arrange
        var yesterday = DateTime.UtcNow.Date.AddDays(-1);
        var userStat = new UserMeta { Id = "user1", LastLearningDay = yesterday, CurrentStreak = 9, LongestStreak = 14 };
        var command = new UpdateUsersStreakCommand { UserId = "user1" };

        _mockRepository.Setup(repo => repo.GetById(command.UserId)).ReturnsAsync(userStat);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        userStat.CurrentStreak.Should().Be(10);
        userStat.LongestStreak.Should().Be(14);
        response.Message.content.Should().Be(MessageCommon.UpdateSuccesfully);
    }


    [Fact]
    public async Task Handle_ContinuousLearning_ShouldIncreaseCurrentStreak()
    {
        // Arrange
        var yesterday = DateTime.UtcNow.Date.AddDays(-1);
        var userStat = new UserMeta { Id = "user1", LastLearningDay = yesterday, CurrentStreak = 3, LongestStreak = 3 };
        var command = new UpdateUsersStreakCommand { UserId = "user1" };

        _mockRepository.Setup(repo => repo.GetById(command.UserId)).ReturnsAsync(userStat);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        userStat.CurrentStreak.Should().Be(4);
        userStat.LongestStreak.Should().Be(4);
        response.Message.content.Should().Be(MessageCommon.UpdateSuccesfully);
    }

    [Fact]
    public async Task Handle_BreakStreak_ShouldResetCurrentStreak()
    {
        // Arrange
        var twoDaysAgo = DateTime.UtcNow.Date.AddDays(-2);
        var userStat = new UserMeta { Id = "user1", LastLearningDay = twoDaysAgo, CurrentStreak = 5, LongestStreak = 5 };
        var command = new UpdateUsersStreakCommand { UserId = "user1" };

        _mockRepository.Setup(repo => repo.GetById(command.UserId)).ReturnsAsync(userStat);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        userStat.CurrentStreak.Should().Be(1);
        userStat.LongestStreak.Should().Be(5);
        response.Message.content.Should().Be(MessageCommon.UpdateSuccesfully);
    }

    [Fact]
    public async Task Handle_LongestStreak_ShouldUpdateCorrectly()
    {
        // Arrange
        var yesterday = DateTime.UtcNow.Date.AddDays(-1);
        var userStat = new UserMeta { Id = "user1", LastLearningDay = yesterday, CurrentStreak = 7, LongestStreak = 5 };
        var command = new UpdateUsersStreakCommand { UserId = "user1" };

        _mockRepository.Setup(repo => repo.GetById(command.UserId)).ReturnsAsync(userStat);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        userStat.CurrentStreak.Should().Be(8);
        userStat.LongestStreak.Should().Be(8);
        response.Message.content.Should().Be(MessageCommon.UpdateSuccesfully);
    }
}
