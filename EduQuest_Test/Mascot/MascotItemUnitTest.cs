using AutoMapper;
using EduQuest_Application.DTO.Response.Mascot;
using EduQuest_Application.UseCases.Mascot.Commands.EquipMacotItem;
using EduQuest_Application.UseCases.Mascot.Commands.PurchaseMascot;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using FluentAssertions;
using Moq;
using System.Net;
using static EduQuest_Domain.Constants.Constants;


namespace EduQuest_Test.Mascot;
public class MascotItemUnitTest
{
    private readonly Mock<IShopItemRepository> _mockShopItemRepo;
    private readonly Mock<IMascotInventoryRepository> _mockMascotInventoryRepo;
    private readonly Mock<IUserStatisticRepository> _mockUserStatisticRepo;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly PurchaseMascotItemCommandHandler _handler;
    private readonly EquipMascotItemCommandHandler _equipMascotHandler;

    public MascotItemUnitTest()
    {
        _mockShopItemRepo = new Mock<IShopItemRepository>();
        _mockMascotInventoryRepo = new Mock<IMascotInventoryRepository>();
        _mockUserStatisticRepo = new Mock<IUserStatisticRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();

        _handler = new PurchaseMascotItemCommandHandler(
            _mockShopItemRepo.Object,
            _mockMascotInventoryRepo.Object,
            _mockUserStatisticRepo.Object,
            _mockMapper.Object,
            _mockUnitOfWork.Object
        );

        _equipMascotHandler = new EquipMascotItemCommandHandler(
            _mockMascotInventoryRepo.Object,
            _mockUnitOfWork.Object,
            _mockMapper.Object
            );
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenShopItemDoesNotExist()
    {
        // Arrange
        var command = new PurchaseMascotItemCommand { UserId = "user1", ShopItemId = "item1" };
        _mockShopItemRepo.Setup(repo => repo.GetById(command.ShopItemId)).ReturnsAsync((ShopItem)null);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.IsError.Should().BeTrue();
        response.Errors.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenUserAlreadyOwnsItem()
    {
        // Arrange
        var command = new PurchaseMascotItemCommand { UserId = "user1", ShopItemId = "item1" };
        var shopItem = new ShopItem { Id = "item1", Price = 100 };

        _mockShopItemRepo.Setup(repo => repo.GetById(command.ShopItemId)).ReturnsAsync(shopItem);
        _mockMascotInventoryRepo.Setup<Task<EduQuest_Domain.Entities.Mascot>>(repo => repo.GetByUserIdAndItemIdAsync(command.UserId, command.ShopItemId))
            .ReturnsAsync(new EduQuest_Domain.Entities.Mascot());

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.IsError.Should().BeTrue();
        response.Errors.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
        response.Message.content.Should().Be(MessageCommon.AlreadyOwnThisItem);
    }

    [Fact]
    public async Task Handle_ShouldAddItemToInventory_WhenUserDoesNotOwnItem()
    {
        // Arrange
        var command = new PurchaseMascotItemCommand { UserId = "user1", ShopItemId = "item1" };
        var shopItem = new ShopItem { Id = "item1", Price = 100 };

        _mockShopItemRepo.Setup(repo => repo.GetById(command.ShopItemId)).ReturnsAsync(shopItem);

        _mockMascotInventoryRepo.Setup<Task<EduQuest_Domain.Entities.Mascot>>(repo => repo.GetByUserIdAndItemIdAsync(command.UserId, command.ShopItemId))
            .ReturnsAsync<IMascotInventoryRepository, EduQuest_Domain.Entities.Mascot>((EduQuest_Domain.Entities.Mascot)null);

        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.IsError.Should().BeFalse();
        response.Message!.content.Should().Be(MessageCommon.PurchaseItemSuccessfully);
        response.Payload.Should().NotBeNull();
    }


    [Fact]
    public async Task Handle_ShouldEquipItem_WhenItemIsNotEquipped()
    {
        // Arrange
        var command = new EquipMascotItemCommand { UserId = "user1", ShopItemId = "item1" };
        var mascotItem = new EduQuest_Domain.Entities.Mascot { UserId = "user1", ShopItemId = "item1", IsEquipped = false };

        _mockMascotInventoryRepo.Setup(repo => repo.GetByUserIdAndItemIdAsync(command.UserId, command.ShopItemId))
            .ReturnsAsync(mascotItem);

        _mockMapper.Setup(mapper => mapper.Map<UserMascotDto>(It.IsAny<EduQuest_Domain.Entities.Mascot>()))
            .Returns(new UserMascotDto());

        // Act
        var response = await _equipMascotHandler.Handle(command, CancellationToken.None);

        // Assert
        mascotItem.IsEquipped.Should().BeTrue();
        response.IsError.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldUnequipItem_WhenItemIsEquipped()
    {
        // Arrange
        var command = new EquipMascotItemCommand { UserId = "user1", ShopItemId = "item1" };
        var mascotItem = new EduQuest_Domain.Entities.Mascot { UserId = "user1", ShopItemId = "item1", IsEquipped = true };

        _mockMascotInventoryRepo.Setup(repo => repo.GetByUserIdAndItemIdAsync(command.UserId, command.ShopItemId))
            .ReturnsAsync(mascotItem);

        _mockMapper.Setup(mapper => mapper.Map<UserMascotDto>(It.IsAny<EduQuest_Domain.Entities.Mascot>()))
            .Returns(new UserMascotDto());

        // Act
        var response = await _equipMascotHandler.Handle(command, CancellationToken.None);

        // Assert
        mascotItem.IsEquipped.Should().BeFalse();
        response.IsError.Should().BeFalse();
    }
}
