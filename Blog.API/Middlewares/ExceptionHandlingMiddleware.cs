using Blog.API.Dtos.Errors;
using Blog.Domain.Exceptions;

namespace Blog.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            context.Response.StatusCode = MapStatusCode(ex);
            context.Response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                Error = ex.GetType().Name,
                Message = ex.Message
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        catch (Exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(new
            {
                Error = "INTERNAL_ERROR",
                Message = "An unexpected error occurred."
            });
        }
    }

    private static int MapStatusCode(DomainException ex) 
    {
        return ex switch
        {
            AlreadyExistsException => StatusCodes.Status409Conflict,
            ValueCannotBeEmptyException => StatusCodes.Status400BadRequest,
            ValueIsInvalidException => StatusCodes.Status400BadRequest,
            ExceedCaractersNumberException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status400BadRequest
        };
    }
}
