using AutoMapper;
using EduQuest_Application.DTO.Response.Badges;
using EduQuest_Application.UseCases.Badges.Commands.CreateBadge;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;
using EduQuest_Domain.Repository.UnitOfWork;
using FluentAssertions;
using Moq;

namespace EduQuest_Test.BadgeTest;

public sealed class BadgeUnitTests
{
    //private readonly IMapper _mapper;
    private readonly CreateBadgeHandler _handler;
    private readonly Mock<IGenericRepository<Badge>> _mockBadgeRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;

    public BadgeUnitTests()
    {
        _mockBadgeRepository = new Mock<IGenericRepository<Badge>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateBadgeHandler(_mockBadgeRepository.Object, _mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GivenValidCommand_ShouldCreateNewBadge_WhenBasketDoesNotExist()
    {
        // Arrange
        var command = new CreateBadgeCommand
        {
            Name = "Test Badge",
            Description = "Test Description",
            IconUrl = "http://example.com/icon.png",
            Color = "#FFFFFF"
        };

        var createdBadge = new Badge
        {
            Id = Guid.NewGuid().ToString(),
            Name = command.Name,
            Description = command.Description,
            IconUrl = command.IconUrl,
            Color = command.Color,
            CreatedAt = DateTime.UtcNow
        };

        var badgeDto = new BadgeDto
        {
            Id = createdBadge.Id,
            Name = createdBadge.Name,
            Description = createdBadge.Description,
            IconUrl = createdBadge.IconUrl,
            Color = createdBadge.Color
        };

        _mockMapper.Setup(m => m.Map<BadgeDto>(It.IsAny<Badge>())).Returns(badgeDto);
        _mockBadgeRepository.Setup(r => r.Add(It.IsAny<Badge>()));
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);


        //Act 
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Payload.Should().BeEquivalentTo(badgeDto);

    }

    [Theory]
    [InlineData(null, "Valid Description")]
    [InlineData("", "Valid Description")]
    [InlineData("Valid Name", null)]
    [InlineData("Valid Name", "")]
    public async Task GivenInvalidCommand_ShouldNotCreateNewBadge_WhenOneOfTheFieldIsMissing(string name, string description)
    {
        // Arrange
        var command = new CreateBadgeCommand
        {
            Name = name,
            Description = description,
            IconUrl = "http://example.com/icon.png",
            Color = "#FFFFFF"
        };

        //Act 
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Payload.Should().BeNull();

    }
}
