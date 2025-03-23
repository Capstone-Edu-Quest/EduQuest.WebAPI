using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.UseCases.Authenticate.Commands.LogOut;
using EduQuest_Application.UseCases.Authenticate.Commands.RefreshToken;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using FluentAssertions;
using Moq;
using System.Net;
using System.Security.Claims;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Test.AuthenticationTest;
public class AuthenticationUnitTests
{
    private readonly Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IUserMetaRepository> _mockUserStatisticRepository;
    private readonly Mock<IJwtProvider> _mockJwtProvider;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRedisCaching> _mockRedisCaching;
    private readonly SignOutCommandHandler _signOutHandler;
    private readonly RefreshTokenQueryHandler _refreshTokenHandler;

    public AuthenticationUnitTests()
    {
        _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockUserStatisticRepository = new Mock<IUserMetaRepository>();
        _mockJwtProvider = new Mock<IJwtProvider>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockRedisCaching = new Mock<IRedisCaching>();

        _signOutHandler = new SignOutCommandHandler(
            _mockRefreshTokenRepository.Object,
            _mockRedisCaching.Object,
            _mockUnitOfWork.Object
        );

        _refreshTokenHandler = new RefreshTokenQueryHandler(
            _mockUserRepository.Object,
            _mockUserStatisticRepository.Object,
            _mockRefreshTokenRepository.Object,
            _mockJwtProvider.Object,
            _mockUnitOfWork.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserTokenDoesNotExist()
    {
        //Arrange
        var command = new SignOutCommand { userId = "user1", accessToken = "token123" };
        _mockRefreshTokenRepository.Setup(repo => repo.GetUserByIdAsync(command.userId))
            .ReturnsAsync((RefreshToken)null);
        //Act
        var response = await _signOutHandler.Handle(command, CancellationToken.None);
        //Assert
        response.IsError.Should().BeTrue();
        response.Errors.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        response.Message.content.Should().Be(MessageCommon.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldDeleteTokenAndCache_WhenTokenExists()
    {
        //Arrange
        var command = new SignOutCommand { userId = "user1", accessToken = "token123" };
        var refreshToken = new RefreshToken { Id = "tokenId1", UserId = "user1", Token = "token123" };

        _mockRefreshTokenRepository.Setup(repo => repo.GetUserByIdAsync(command.userId))
            .ReturnsAsync(refreshToken);

        _mockRedisCaching.Setup(redis => redis.SetAsync(It.IsAny<string>(), true, 5))
            .Returns(Task.CompletedTask);

        _mockRefreshTokenRepository.Setup(repo => repo.Delete(refreshToken.Id))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        //Act
        var response = await _signOutHandler.Handle(command, CancellationToken.None);

        //Assert
        response.IsError.Should().BeFalse();
        response.Message.content.Should().Be(MessageCommon.LogOutSuccessfully);
        _mockRedisCaching.Verify(redis => redis.SetAsync(It.IsAny<string>(), true, 5), Times.Once);
        _mockRefreshTokenRepository.Verify(repo => repo.Delete(refreshToken.Id), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenTokenIsInvalid()
    {
        // Arrange
        var query = new RefreshTokenQuery { AccessToken = "invalidToken", RefreshToken = "refresh123" };

        _mockJwtProvider.Setup(jwt => jwt.GetPrincipalFromExpiredToken(query.AccessToken))
            .Returns((ClaimsPrincipal)null!); // Giả lập trả về null

        // Act
        var response = await _refreshTokenHandler.Handle(query, CancellationToken.None);

        // Assert
        response.IsError.Should().BeTrue();
        response.Errors.Should().NotBeNull();
        response.Errors.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        response.Errors.Message.Should().Be(MessageCommon.InvalidToken);
    }


    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenUserIsBlocked()
    {
        //Arrange
        var query = new RefreshTokenQuery { AccessToken = "validToken", RefreshToken = "refresh123" };
        var refreshToken = new RefreshToken { Id = "tokenId1", UserId = "user1", Token = "token123" };

        var claims = new List<Claim>
        {
            new Claim(UserClaimType.UserId, "user1")
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        _mockJwtProvider.Setup(jwt => jwt.GetPrincipalFromExpiredToken(query.AccessToken))
            .Returns(principal);

        _mockRefreshTokenRepository.Setup(repo => repo.GetUserByIdAsync("user1"))
            .ReturnsAsync(refreshToken);

        var user = new User { Id = "user1", Email = "test@example.com", Status = AccountStatus.Blocked.ToString() };
        _mockUserRepository.Setup(repo => repo.GetById("user1"))
            .ReturnsAsync(user);

        //Act
        var response = await _refreshTokenHandler.Handle(query, CancellationToken.None);

        //Assert
        response.IsError.Should().BeTrue();
        response.Errors.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        response.Message.content.Should().Be(MessageCommon.Blocked);
    }


    [Fact]
    public async Task Handle_ShouldReturnNewTokens_WhenRefreshTokenIsValid()
    {
        //Arrange
        var query = new RefreshTokenQuery { AccessToken = "validToken", RefreshToken = "refresh123" };
        var user = new User { Id = "user1", Email = "test@example.com", Status = AccountStatus.Active.ToString() };

        var claims = new List<Claim>
        {
            new Claim(UserClaimType.UserId, "user1")
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        _mockJwtProvider.Setup(jwt => jwt.GetPrincipalFromExpiredToken(query.AccessToken))
            .Returns(principal);

        _mockRefreshTokenRepository.Setup(repo => repo.GetUserByIdAsync("user1"))
            .ReturnsAsync(new RefreshToken { Id = "tokenId1", UserId = "user1", Token = "token123", ExpireAt = DateTime.UtcNow.AddMinutes(5) });


        _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<string>()))
            .ReturnsAsync(user);

        _mockRefreshTokenRepository.Setup(repo => repo.Delete(""))
            .Returns(Task.CompletedTask);

        var tokenResponse = new TokenResponseDTO { AccessToken = "newAccessToken", RefreshToken = "newRefreshToken" };
        _mockJwtProvider.Setup(jwt => jwt.GenerateAccessRefreshTokens(user.Id, user.Email))
            .ReturnsAsync(tokenResponse);

        //Act
        var response = await _refreshTokenHandler.Handle(query, CancellationToken.None);

        //Assert
        response.IsError.Should().BeFalse();
        response.Message.content.Should().Be(MessageCommon.TokenRefreshSuccess);
        response.Payload.Should().BeEquivalentTo(tokenResponse);
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenTokenIsExpired()
    {
        // Arrange
        var query = new RefreshTokenQuery { AccessToken = "validToken", RefreshToken = "expiredRefreshToken" };

        var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
        new Claim(UserClaimType.UserId, "user1")
        }));

        _mockJwtProvider.Setup(jwt => jwt.GetPrincipalFromExpiredToken(query.AccessToken))
            .Returns(principal);

        var user = new User { Id = "user1", Email = "test@example.com", Status = AccountStatus.Active.ToString() };
        _mockUserRepository.Setup(repo => repo.GetById("user1"))
            .ReturnsAsync(user);

        var expiredToken = new RefreshToken
        {
            Id = "refreshToken1",
            UserId = "user1",
            Token = "expiredRefreshToken",
            ExpireAt = DateTime.UtcNow.AddMinutes(-5) // Token expired 5 minutes ago
        };
        _mockRefreshTokenRepository.Setup(repo => repo.GetUserByIdAsync("user1"))
            .ReturnsAsync(expiredToken);

        // Act
        var response = await _refreshTokenHandler.Handle(query, CancellationToken.None);

        // Assert
        response.IsError.Should().BeTrue();
        response.Errors.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        response.Message.content.Should().Be(MessageCommon.TokenExpired);
    }
}

