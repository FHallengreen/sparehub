using System.ComponentModel.DataAnnotations;
using Shared.Exceptions;

namespace Server.Middleware;

public class ValidationExceptionMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = new ValidationErrorResponse
            {
                Message = "Validation Failed",
                Errors = new Dictionary<string, string[]>()
            };
            response.Errors.Add("ValidationError", [ex.Message]);

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                Message = ex.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                Message = "An unexpected error occurred.",
                Details = hostEnvironment.IsDevelopment() ? ex.ToString() : null
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}