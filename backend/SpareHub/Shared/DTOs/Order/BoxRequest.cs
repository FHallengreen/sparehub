using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Order;

public class BoxRequest
{
    [RegularExpression(@"^([0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})?$",
        ErrorMessage = "Id must be a valid GUID or empty.")]
    public string? Id { get; init; }

    [Required(ErrorMessage = "Length is required.")]
    [Range(1, 999999, ErrorMessage = "Length must be between 1 and 999999 cm.")]
    [DefaultValue(1)]
    public int Length { get; init; }

    [Required(ErrorMessage = "Width is required.")]
    [Range(1, 999999, ErrorMessage = "Width must be between 1 and 999999 cm.")]
    [DefaultValue(1)]
    public int Width { get; init; }

    [Required(ErrorMessage = "Height is required.")]
    [Range(1, 999999, ErrorMessage = "Height must be between 1 and 999999 cm.")]
    [DefaultValue(1)]
    public int Height { get; init; }

    [Required(ErrorMessage = "Weight is required.")]
    [Range(0.1, 99999999, ErrorMessage = "Weight must be between 0.1 and 99999999 kg.")]
    [DefaultValue(1)]
    public double Weight { get; init; }
}