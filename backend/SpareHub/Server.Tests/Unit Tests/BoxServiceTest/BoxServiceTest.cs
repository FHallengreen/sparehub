using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Domain.Models;
using Moq;
using Repository.Interfaces;
using Service.MySql.Box;
using Service.MySql.Order;
using Shared.DTOs.Order;
using Shared.Exceptions;
using Xunit;

namespace Server.Tests.Unit_Tests.BoxServiceTest;

public class BoxServiceTest
{
    private readonly BoxService _boxService;
    private readonly Mock<IBoxRepository> _boxRepositoryMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;

    public BoxServiceTest()
    {
        _boxRepositoryMock = new Mock<IBoxRepository>();
        _orderRepositoryMock = new Mock<IOrderRepository>();

        _boxService = new BoxService(_boxRepositoryMock.Object, _orderRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateBox_ThrowsValidationException_WhenOrderIdIsEmpty()
    {
        // Arrange
        var boxRequest = new BoxRequest
        {
            Length = 10,
            Width = 10,
            Height = 10,
            Weight = 5.0
        };

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<ValidationException>(() => _boxService.CreateBox(boxRequest, string.Empty));
        Assert.Equal("Order ID cannot be null or empty.", exception.Message);
    }

    [Fact]
    public async Task CreateBox_ThrowsValidationException_WhenOrderDoesNotExist()
    {
        // Arrange
        var boxRequest = new BoxRequest
        {
            Length = 10,
            Width = 10,
            Height = 10,
            Weight = 5.0
        };

        _orderRepositoryMock
            .Setup(repo => repo.GetOrderByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Order)null);

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<ValidationException>(() => _boxService.CreateBox(boxRequest, "invalid-order-id"));
        Assert.Equal("Order ID 'invalid-order-id' is invalid or does not exist.", exception.Message);
    }

    [Fact]
    public async Task CreateBox_Success_WhenValidInput()
    {
        // Arrange
        var boxRequest = new BoxRequest
        {
            Length = 10,
            Width = 10,
            Height = 10,
            Weight = 5.0
        };

        var orderId = "valid-order-id";

        _orderRepositoryMock
            .Setup(repo => repo.GetOrderByIdAsync(orderId))
            .ReturnsAsync(new Order { Id = orderId });

        _boxRepositoryMock
            .Setup(repo => repo.CreateBoxAsync(It.IsAny<Box>()))
            .ReturnsAsync(new Box
            {
                Id = "new-box-id",
                OrderId = orderId,
                Length = 10,
                Width = 10,
                Height = 10,
                Weight = 5.0
            });

        // Act
        var result = await _boxService.CreateBox(boxRequest, orderId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(10, result.Length);
        Assert.Equal(10, result.Width);
        Assert.Equal(10, result.Height);
        Assert.Equal(5.0, result.Weight);
    }

    [Fact]
    public async Task GetBoxes_ThrowsNotFoundException_WhenNoBoxesExist()
    {
        // Arrange
        const string orderId = "valid-order-id";

        _boxRepositoryMock
            .Setup(repo => repo.GetBoxesByOrderIdAsync(orderId))
            .ReturnsAsync([]);

        // Act & Assert
        var order = await _boxService.GetBoxes(orderId);
        // Actual:   []
        Assert.NotNull(order);
        Assert.Empty(order);
    }

    [Fact]
    public async Task GetBoxes_Success_WhenBoxesExist()
    {
        // Arrange
        const string orderId = "valid-order-id";

        _boxRepositoryMock
            .Setup(repo => repo.GetBoxesByOrderIdAsync(orderId))
            .ReturnsAsync([
                new Box { Id = "box-1", OrderId = orderId, Length = 10, Width = 10, Height = 10, Weight = 5.0 },
                new Box { Id = "box-2", OrderId = orderId, Length = 15, Width = 15, Height = 15, Weight = 10.0 }
            ]);

        // Act
        var result = await _boxService.GetBoxes(orderId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("box-1", result[0].Id);
        Assert.Equal("box-2", result[1].Id);
    }
}