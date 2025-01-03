using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shared.DTOs.Order;
using Xunit;

namespace Server.Tests.Services.UnitTests.BoxTests;

public class BoxRequestValidationTests
{

    private static IList<ValidationResult> ValidateModel(object model)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, context, results, true);
        return results;
    }

    [Theory]
    [InlineData(0, 50, 30, 5.0, "Length must be between 1 and 999999 cm.")] // Invalid Length
    [InlineData(100, -10, 30, 5.0, "Width must be between 1 and 999999 cm.")] // Invalid Width
    [InlineData(100, 50, 0, 5.0, "Height must be between 1 and 999999 cm.")] // Invalid Height
    [InlineData(100, 50, 30, 0.0, "Weight must be between 0.1 and 99999999 kg.")] // Invalid Weight
    [InlineData(100, 50, 30, 100000000, "Weight must be between 0.1 and 99999999 kg.")] // Invalid Weight
    public void BoxRequest_InvalidModel_ReturnsValidationError(
        int length, int width, int height, double weight, string expectedErrorMessage)
    {
        // Arrange
        var boxRequest = new BoxRequest
        {
            Length = length,
            Width = width,
            Height = height,
            Weight = weight
        };

        // Act
        var validationResults = ValidateModel(boxRequest);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, vr => vr.ErrorMessage == expectedErrorMessage);
    }

    [Theory]
    [InlineData(100, 50, 30, 5.0)] // Valid box
    [InlineData(1, 1, 1, 0.1)] // Edge of valid range lower bound
    [InlineData(999999, 999999, 999999, 99999999)] // Edge of valid range upper bound
    public void BoxRequest_ValidModel_PassesValidation(
        int length, int width, int height, double weight)
    {
        // Arrange
        var boxRequest = new BoxRequest
        {
            Length = length,
            Width = width,
            Height = height,
            Weight = weight
        };

        // Act
        var validationResults = ValidateModel(boxRequest);

        // Assert
        Assert.Empty(validationResults);
    }
}