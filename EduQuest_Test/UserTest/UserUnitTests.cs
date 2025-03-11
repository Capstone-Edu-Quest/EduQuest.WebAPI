using AutoMapper;
using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.UseCases.Users.Commands.SwitchRole;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using FluentAssertions;
using Moq;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Test.UserTest;

public class UserUnitTests
{
    private readonly SwitchRoleCommandHandler _handler;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IJwtProvider> _mockJwtProvider;
    private readonly Mock<IRedisCaching> _mockRedis;
    private readonly Mock<IMapper> _mockMapper;

    public UserUnitTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockRedis = new Mock<IRedisCaching>();
        _mockJwtProvider = new Mock<IJwtProvider>();
        _mockMapper = new Mock<IMapper>();
        _handler = new SwitchRoleCommandHandler(_mockUserRepository.Object, _mockRedis.Object, _mockJwtProvider.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnUpdatedAccessToken_WhenRoleSwitchIsSuccessful()
    {
        // Arrange
        var user = new User { Id = "user123", Email = "test@email" };
        var command = new SwitchRoleCommand
        {
            userId = user.Id,
            RoleId = "newRole123",
            accessToken = "testToken"
        };

        var expectedAccessToken = "newAccessToken";

        _mockUserRepository.Setup(repo => repo.GetById(command.userId)).ReturnsAsync(user);
        _mockJwtProvider.Setup(provider => provider.GenerateAccessToken(user.Email!)).ReturnsAsync(expectedAccessToken);
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _mockRedis.Setup(cache => cache.SetAsync(It.IsAny<string>(), true, 5)).Returns(Task.CompletedTask);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.IsError.Should().BeFalse();
        response.Payload.Should().NotBeNull();
        response.Errors.Should().BeNull();
        response.Message!.content.Should().Be(MessageCommon.UpdateSuccesfully);
        response.Payload.GetType().GetProperty("AccessToken")!.GetValue(response.Payload)!.Should().Be(expectedAccessToken);

        _mockUserRepository.Verify(repo => repo.Update(user), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockRedis.Verify(cache => cache.SetAsync($"Token_{command.accessToken}", true, 5), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new SwitchRoleCommand
        {
            userId = "nonExistentUser",
            RoleId = "newRole123",
            accessToken = "testToken"
        };

        _mockUserRepository.Setup(repo => repo.GetById(command.userId)).ReturnsAsync((User)null);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.IsError.Should().BeTrue();
        response.Errors.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        response.Message!.content.Should().Be(MessageCommon.NotFound);
        response.Payload.Should().BeNull();

        _mockUserRepository.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

}
