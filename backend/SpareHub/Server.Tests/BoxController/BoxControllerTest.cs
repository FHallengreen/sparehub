using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Persistence.MySql;
using Persistence.MySql.SparehubDbContext;
using Shared.DTOs.Order;
using Shared.Exceptions;
using Shared.Order;
using Xunit;

namespace Server.Tests.BoxController;

[TestSubject(typeof(Server.BoxController.BoxController))]
public class BoxControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly SpareHubDbContext _dbContext;
    private readonly OrderEntity _seededOrder;

    public BoxControllerTests(WebApplicationFactory<Program> factory)
    {
        var services = factory.Services.CreateScope().ServiceProvider;
        _dbContext = services.GetRequiredService<SpareHubDbContext>();
        _client = factory.CreateClient();
        _seededOrder = SeedDataAsync().Result;
    }

    [Fact]
    public async Task CreateBox_ReturnsCreated_ForValidInput()
    {
        var boxRequest = new BoxRequest
        {
            Length = 10,
            Width = 10,
            Height = 10,
            Weight = 5.0
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/order/{_seededOrder.Id}/box", boxRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var boxResponse = await response.Content.ReadFromJsonAsync<BoxResponse>();
        Assert.NotNull(boxResponse);
        Assert.Equal(_seededOrder.Id.ToString(), boxResponse.OrderId);
        Assert.Equal(boxRequest.Length, boxResponse.Length);
        Assert.Equal(boxRequest.Width, boxResponse.Width);
        Assert.Equal(boxRequest.Height, boxResponse.Height);
        Assert.Equal(boxRequest.Weight, boxResponse.Weight);
        Assert.False(string.IsNullOrEmpty(boxResponse.Id));
    }


    [Fact]
    public async Task CreateBox_ReturnsBadRequest_ForNegativeLength()
    {
        // Arrange
        var boxRequest = new BoxRequest
        {
            Length = -10,
            Width = 10,
            Height = 10,
            Weight = 5.0
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/order/{_seededOrder.Id}/box", boxRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var errorResponse = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(errorResponse);
        Assert.Contains("Length must be between 1 and 999999 cm.", errorResponse.Errors["Length"]);
    }

    [Fact]
    public async Task CreateBox_ReturnsBadRequest_ForNonExistentOrder()
    {
        var boxRequest = new BoxRequest
        {
            Length = 10,
            Width = 10,
            Height = 10,
            Weight = 5.0
        };

        var nonExistentOrderId = "99999";
        var response = await _client.PostAsJsonAsync($"/api/order/{nonExistentOrderId}/box", boxRequest);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var errorResponse = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
        Assert.NotNull(errorResponse);
        Assert.Equal("Validation Failed", errorResponse.Message);
        Assert.Contains($"Order ID '{nonExistentOrderId}' is invalid or does not exist.", errorResponse.Errors["ValidationError"]);
    }

    [Theory]
    [InlineData(-1, 10, 10, 5.0, "Length must be between 1 and 999999 cm.")]
    [InlineData(10, -10, 10, 5.0, "Width must be between 1 and 999999 cm.")]
    [InlineData(10, 10, -10, 5.0, "Height must be between 1 and 999999 cm.")]
    [InlineData(10, 10, 10, -5.0, "Weight must be between 0.1 and 99999999 kg.")]
    public async Task CreateBox_ReturnsBadRequest_ForInvalidFields(
        int length, int width, int height, double weight, string expectedError)
    {
        var boxRequest = new BoxRequest
        {
            Length = length,
            Width = width,
            Height = height,
            Weight = weight
        };

        var response = await _client.PostAsJsonAsync($"/api/order/{_seededOrder.Id}/box", boxRequest);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var errorResponse = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(errorResponse);
        Assert.Contains(expectedError, errorResponse.Errors.Values.SelectMany(v => v));
    }

    [Theory]
    [InlineData(1, 1, 1, 0.1)] // Minimum valid values
    [InlineData(999999, 999999, 999999, 99999999)] // Maximum valid values
    public async Task CreateBox_ReturnsCreated_ForBoundaryValues(int length, int width, int height, double weight)
    {
        var boxRequest = new BoxRequest
        {
            Length = length,
            Width = width,
            Height = height,
            Weight = weight
        };

        var response = await _client.PostAsJsonAsync($"/api/order/{_seededOrder.Id}/box", boxRequest);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var boxResponse = await response.Content.ReadFromJsonAsync<BoxResponse>();
        Assert.NotNull(boxResponse);
        Assert.Equal(length, boxResponse.Length);
        Assert.Equal(width, boxResponse.Width);
        Assert.Equal(height, boxResponse.Height);
        Assert.Equal(weight, boxResponse.Weight);
    }


    private async Task<OrderEntity> SeedDataAsync()
    {
        var order = _dbContext.Orders.FirstOrDefault(o => o.OrderNumber == "ORD123");

        if (order == null)
        {
            order = new OrderEntity
            {
                OrderNumber = "ORD123",
                SupplierOrderNumber = "SUP123",
                SupplierId = 1,
                VesselId = 1,
                WarehouseId = 1,
                ExpectedReadiness = DateTime.Now.AddDays(5),
                OrderStatus = "Pending"
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
        }

        return order;
    }
}