using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Order;

public record BoxRequest
{
    [RegularExpression(@"^([0-9a-fA-F]{24})?$",
        ErrorMessage = "Id must be a valid ID or empty.")]
    public string? Id { get; init; }

    [Required(ErrorMessage = "Length is required.")]
    [Range(1, 999999, ErrorMessage = "Length must be between 1 and 999999 cm.")]
    [DefaultValue(1)]
    public required int Length { get; init; }

    [Required(ErrorMessage = "Width is required.")]
    [Range(1, 999999, ErrorMessage = "Width must be between 1 and 999999 cm.")]
    [DefaultValue(1)]
    public required int Width { get; init; }

    [Required(ErrorMessage = "Height is required.")]
    [Range(1, 999999, ErrorMessage = "Height must be between 1 and 999999 cm.")]
    [DefaultValue(1)]
    public required int Height { get; init; }

    [Required(ErrorMessage = "Weight is required.")]
    [Range(0.1, 99999999, ErrorMessage = "Weight must be between 0.1 and 99999999 kg.")]
    [DefaultValue(1)]
    public required double Weight { get; init; }
}