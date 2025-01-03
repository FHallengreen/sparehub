using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Moq;
using Moq.Protected;
using Service.Services.Tracking;
using Shared.Exceptions;
using Xunit;

namespace Server.Tests.Services.UnitTests.Tracking;

[TestSubject(typeof(TrackingService))]
public class TrackingServiceTest
{
    [Fact]
    public async Task GetTrackingStatusAsync_ShouldReturnCorrectData_WhenApiResponseIsValid()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{
                    ""shipments"": [
                        {
                            ""events"": [
                                {
                                    ""description"": ""Delivered"",
                                    ""location"": {
                                        ""address"": { ""addressLocality"": ""Copenhagen"" }
                                    },
                                    ""timestamp"": ""2025-01-02T16:30:00+01:00""
                                }
                            ],
                            ""estimatedTimeOfDelivery"": ""2025-01-02T18:00:00Z""
                        }
                    ]
                }")
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var service = new TrackingService(httpClient);

        // Act
        var result = await service.GetTrackingStatusAsync("1234567890");

        // Assert
        Assert.Equal("Delivered", result.StatusDescription);
        Assert.Equal("Copenhagen", result.Location);
        Assert.Equal("02-01-2025 16:30", result.Timestamp);
        Assert.Equal("02-01-2025 18:00", result.EstimatedDelivery);
    }

    [Fact]
    public async Task GetTrackingStatusAsync_ShouldThrowException_WhenResponseIsError()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("Bad Request")
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var service = new TrackingService(httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => service.GetTrackingStatusAsync("1234567890"));
    }

    [Fact]
    public async Task GetTrackingStatusAsync_ShouldThrowNotFoundException_WhenShipmentsArrayIsEmpty()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{ ""shipments"": [] }")
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var service = new TrackingService(httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetTrackingStatusAsync("1234567890"));
    }

    [Fact]
    public async Task GetTrackingStatusAsync_ShouldThrowNotFoundException_WhenEventsArrayIsEmpty()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{
                ""shipments"": [
                    {
                        ""events"": []
                    }
                ]
            }")
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var service = new TrackingService(httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetTrackingStatusAsync("1234567890"));
    }

    [Fact]
    public async Task GetTrackingStatusAsync_ShouldThrowValidationException_WhenTimestampIsInvalid()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{
                ""shipments"": [
                    {
                        ""events"": [
                            {
                                ""description"": ""Delivered"",
                                ""location"": {
                                    ""address"": { ""addressLocality"": ""Copenhagen"" }
                                },
                                ""timestamp"": ""invalid-timestamp""
                            }
                        ]
                    }
                ]
            }")
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var service = new TrackingService(httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => service.GetTrackingStatusAsync("1234567890"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("1234567890123456789012345678901234567890")]
    [InlineData("123456789")]
    [InlineData("12345678910")]
    public async Task GetTrackingStatusAsync_ShouldThrowValidationException_ForInvalidTrackingNumbers(
        string trackingNumber)
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(mockHandler.Object);
        var service = new TrackingService(httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => service.GetTrackingStatusAsync(trackingNumber));
    }

    [Theory]
    [InlineData("2025-01-02T15:30:00Z", "02-01-2025 15:30")]
    [InlineData("2025-01-03T02:46:00+01:00", "03-01-2025 02:46")]
    [InlineData("2025-01-01T00:00:00+08:00", "01-01-2025 00:00")]
    [InlineData("2025-01-01T00:00:00-08:00", "01-01-2025 00:00")]
    [InlineData("2025-01-01T00:00:00+14:00", "01-01-2025 00:00")]
    [InlineData("2025-01-01T00:00:00-12:00", "01-01-2025 00:00")]
    public async Task GetTrackingStatusAsync_ShouldHandleBoundaryTimestamps(string timestamp, string expected)
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent($@"{{
                ""shipments"": [
                    {{
                        ""events"": [
                            {{
                                ""description"": ""Delivered"",
                                ""location"": {{
                                    ""address"": {{ ""addressLocality"": ""Copenhagen"" }}
                                }},
                                ""timestamp"": ""{timestamp}""
                            }}
                        ]
                    }}
                ]
            }}")
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var service = new TrackingService(httpClient);

        // Act
        var result = await service.GetTrackingStatusAsync("1234567890");

        // Assert
        Assert.Equal(expected, result.Timestamp);
    }


    [Fact]
    public async Task GetTrackingStatusAsync_ShouldReturnCorrectData_ForFullApiResponse()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{
                ""shipments"": [
                    {
                        ""events"": [
                            {
                                ""timestamp"": ""2025-01-03T07:24:00+01:00"",
                                ""location"": {
                                    ""address"": { ""addressLocality"": ""AMSTERDAM - NETHERLANDS, THE"" }
                                },
                                ""statusCode"": ""transit"",
                                ""description"": ""Arrived at DHL Delivery Facility  AMSTERDAM - NETHERLANDS, THE""
                            },
                            {
                                ""timestamp"": ""2025-01-03T04:16:00+01:00"",
                                ""location"": {
                                    ""address"": { ""addressLocality"": ""BRUSSELS - BELGIUM"" }
                                },
                                ""statusCode"": ""transit"",
                                ""description"": ""Shipment has departed from a DHL facility BRUSSELS - BELGIUM""
                            },
                            {
                                ""timestamp"": ""2025-01-03T02:46:00+01:00"",
                                ""location"": {
                                    ""address"": { ""addressLocality"": ""BRUSSELS - BELGIUM"" }
                                },
                                ""statusCode"": ""transit"",
                                ""description"": ""Processed at BRUSSELS - BELGIUM""
                            },
                            {
                                ""timestamp"": ""2025-01-03T01:27:00+01:00"",
                                ""location"": {
                                    ""address"": { ""addressLocality"": ""BRUSSELS - BELGIUM"" }
                                },
                                ""statusCode"": ""transit"",
                                ""description"": ""Arrived at DHL Sort Facility  BRUSSELS - BELGIUM""
                            },
                            {
                                ""timestamp"": ""2025-01-02T23:22:00+01:00"",
                                ""location"": {
                                    ""address"": { ""addressLocality"": ""VITORIA - SPAIN"" }
                                },
                                ""statusCode"": ""transit"",
                                ""description"": ""Shipment has departed from a DHL facility VITORIA - SPAIN""
                            },
                            {
                                ""timestamp"": ""2025-01-02T20:05:00+01:00"",
                                ""location"": {
                                    ""address"": { ""addressLocality"": ""VITORIA - SPAIN"" }
                                },
                                ""statusCode"": ""transit"",
                                ""description"": ""Processed at VITORIA - SPAIN""
                            },
                            {
                                ""timestamp"": ""2025-01-02T19:35:00+01:00"",
                                ""location"": {
                                    ""address"": { ""addressLocality"": ""VITORIA - SPAIN"" }
                                },
                                ""statusCode"": ""transit"",
                                ""description"": ""Arrived at DHL Sort Facility  VITORIA - SPAIN""
                            },
                            {
                                ""timestamp"": ""2025-01-02T13:30:00+01:00"",
                                ""location"": {
                                    ""address"": { ""addressLocality"": ""VITORIA - SPAIN"" }
                                },
                                ""statusCode"": ""transit"",
                                ""description"": ""Shipment picked up""
                            }
                        ],
                        ""estimatedTimeOfDelivery"": ""2025-01-03""
                    }
                ]
            }")
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var service = new TrackingService(httpClient);

        // Act
        var result = await service.GetTrackingStatusAsync("4680451941");

        // Assert
        Assert.Equal("Arrived at DHL Delivery Facility  AMSTERDAM - NETHERLANDS, THE", result.StatusDescription);
        Assert.Equal("AMSTERDAM - NETHERLANDS, THE", result.Location);
        Assert.Equal("03-01-2025 07:24", result.Timestamp);
        Assert.Equal("03-01-2025 00:00", result.EstimatedDelivery);
    }
}