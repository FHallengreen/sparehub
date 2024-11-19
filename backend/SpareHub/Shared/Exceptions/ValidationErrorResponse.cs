namespace Shared.Exceptions;

public class ValidationErrorResponse
{
    public string Message { get; set; } = "Validation Failed";
    public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}