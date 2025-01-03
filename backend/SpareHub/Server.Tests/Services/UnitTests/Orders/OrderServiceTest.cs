using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;
using JetBrains.Annotations;
using Moq;
using Persistence.MySql;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Services.Order;
using Shared.Exceptions;
using Xunit;

namespace Server.Tests.Services.UnitTests.Orders;

[TestSubject(typeof(OrderService))]
public class OrderServiceTest
{
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly OrderService _orderService;

    public OrderServiceTest()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        var mockBoxService = new Mock<IBoxService>();
        var mockMapper = new Mock<IMapper>();

        _orderService = new OrderService(
            _mockOrderRepository.Object,
            mockBoxService.Object,
            mockMapper.Object
        );
    }

    private static List<OrderEntity> GetTestOrderEntities()
    {
        return
        [
            TestDataHelper.CreateOrderEntity(
                id: 1,
                orderNumber: "ORD-123",
                orderStatus: OrderStatus.Pending
            ),

            TestDataHelper.CreateOrderEntity(
                id: 2,
                orderNumber: "ORD-456",
                orderStatus: OrderStatus.Delivered
            )
        ];
    }

    private static List<Order> MapEntitiesToOrders(IEnumerable<OrderEntity> orderEntities)
    {
        return orderEntities.Select(entity => new Order
        {
            Id = entity.Id.ToString(),
            OrderNumber = entity.OrderNumber,
            OrderStatus = Enum.Parse<OrderStatus>(entity.OrderStatus, true)
        }).ToList();
    }

    [Fact]
    public async Task GetOrders_ShouldReturnNonActiveOrders_WhenNoSearchTermsProvided()
    {
        // Arrange
        var orderEntities = GetTestOrderEntities();
        var orders = MapEntitiesToOrders(orderEntities);

        _mockOrderRepository
            .Setup(repo => repo.GetActiveOrders())
            .ReturnsAsync(orders);

        // Act
        var result = await _orderService.GetOrders();

        // Assert
        var orderTableResponses = result.ToList();
        Assert.Equal(2, orderTableResponses.Count);
        Assert.Contains(orderTableResponses, o => o.OrderNumber == "ORD-123" && o.OrderStatus == OrderStatus.Pending);
        Assert.Contains(orderTableResponses, o => o.OrderNumber == "ORD-456" && o.OrderStatus == OrderStatus.Delivered);
    }

    [Fact]
    public async Task GetOrders_ShouldReturnDeliveredOrCancelledOrders_WhenSearchTermsIncludeDeliveredOrCancelled()
    {
        // Arrange
        var orderEntities = GetTestOrderEntities();
        var orders = MapEntitiesToOrders(orderEntities);

        _mockOrderRepository
            .Setup(repo => repo.GetOrdersAsync())
            .ReturnsAsync(orders);

        var searchTerms = new List<string> { "Delivered" };

        // Act
        var result = await _orderService.GetOrders(searchTerms);

        // Assert
        var orderTableResponses = result.ToList();
        Assert.Single(orderTableResponses);
        Assert.Contains(orderTableResponses, o => o.OrderNumber == "ORD-456" && o.OrderStatus == OrderStatus.Delivered);
    }

    [Fact]
    public async Task GetOrders_ShouldFetchOrdersBasedOnSearchTerms_WhenOtherSearchTermsProvided()
    {
        // Arrange
        var orderEntities = GetTestOrderEntities();
        var orders = MapEntitiesToOrders(orderEntities);

        _mockOrderRepository
            .Setup(repo => repo.GetOrdersAsync())
            .ReturnsAsync(orders);

        var searchTerms = new List<string> { "Pending" };

        // Act
        var result = await _orderService.GetOrders(searchTerms);

        // Assert
        var orderTableResponses = result.ToList();
        Assert.Single(orderTableResponses);
        Assert.Equal("ORD-123", orderTableResponses.First().OrderNumber);
        Assert.Equal(OrderStatus.Pending, orderTableResponses.First().OrderStatus);
    }

    [Fact]
    public async Task GetOrders_ShouldReturnOrdersMatchingWarehouseName()
    {
        // Arrange
        const string searchTerm = "Amsterdam";

        var orderEntities = new List<OrderEntity>
        {
            TestDataHelper.CreateOrderEntity(
                id: 1,
                orderNumber: "ORD-001",
                orderStatus: OrderStatus.Pending,
                warehouse: new WarehouseEntity
                {
                    Name = "Amsterdam Warehouse",
                    Agent = new AgentEntity
                    {
                        Name = "Agency"
                    },
                    Address = new AddressEntity
                    {
                        AddressLine = "null",
                        PostalCode = "null",
                        Country = "null"
                    }
                }
            ),
            TestDataHelper.CreateOrderEntity(
                id: 2,
                orderNumber: "ORD-002",
                orderStatus: OrderStatus.Pending,
                warehouse: new WarehouseEntity
                {
                    Name = "Amsterdam Warehouse",
                    Agent = new AgentEntity
                    {
                        Name = "Agency"
                    },
                    Address = new AddressEntity
                    {
                        AddressLine = "null",
                        PostalCode = "null",
                        Country = "null"
                    }
                }),
            TestDataHelper.CreateOrderEntity(
                id: 2,
                orderNumber: "ORD-002",
                orderStatus: OrderStatus.Pending,
                warehouse: new WarehouseEntity
                {
                    Name = "Osaka Warehouse",
                    Agent = new AgentEntity
                    {
                        Name = "Agency"
                    },
                    Address = new AddressEntity
                    {
                        AddressLine = "null",
                        PostalCode = "null",
                        Country = "null"
                    }
                })
        };

        var orders = orderEntities.Select(e => new Order
        {
            Id = e.Id.ToString(),
            OrderNumber = e.OrderNumber,
            OrderStatus = Enum.Parse<OrderStatus>(e.OrderStatus),
            Warehouse = new Warehouse { Name = e.Warehouse.Name }
        }).ToList();

        _mockOrderRepository
            .Setup(repo => repo.GetActiveOrders())
            .ReturnsAsync(orders);

        _mockOrderRepository
            .Setup(repo => repo.GetOrdersAsync())
            .ReturnsAsync(orders);

        // Act
        var result = await _orderService.GetOrders([searchTerm]);

        // Assert
        var matchedOrders = result.ToList();
        Assert.Equal(2, matchedOrders.Count);
    }

    [Fact]
    public async Task GetOrders_ShouldReturnOrdersMatchingOrderNumber()
    {
        // Arrange
        const string searchTerm = "ORD-002";

        var orderEntities = new List<OrderEntity>
        {
            TestDataHelper.CreateOrderEntity(
                id: 1,
                orderNumber: "ORD-001",
                orderStatus: OrderStatus.Pending
            ),
            TestDataHelper.CreateOrderEntity(
                id: 2,
                orderNumber: "ORD-002",
                orderStatus: OrderStatus.Pending
            )
        };

        var orders = orderEntities.Select(e => new Order
        {
            Id = e.Id.ToString(),
            OrderNumber = e.OrderNumber,
            OrderStatus = Enum.Parse<OrderStatus>(e.OrderStatus)
        }).ToList();

        _mockOrderRepository
            .Setup(repo => repo.GetActiveOrders())
            .ReturnsAsync(orders);

        _mockOrderRepository
            .Setup(repo => repo.GetOrdersAsync())
            .ReturnsAsync(orders);

        // Act
        var result = await _orderService.GetOrders([searchTerm]);

        // Assert
        var matchedOrders = result.ToList();
        Assert.Single(matchedOrders);
        Assert.Equal("ORD-002", matchedOrders.First().OrderNumber);
    }
}