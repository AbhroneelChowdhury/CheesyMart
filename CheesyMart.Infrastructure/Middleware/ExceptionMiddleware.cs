using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using CheesyMart.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace CheesyMart.Infrastructure.Middleware;

public class ExceptionMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await requestDelegate(httpContext);
        }
        catch (Exception ex)
        {
            await HandleException(httpContext, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        string message;
        string statusCode;
        switch (exception)
        {
            case ValidationException validationException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                message = validationException.Message;
                statusCode = HttpStatusCode.BadRequest.ToString();
                break;
            case NotFoundException notFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                message = notFoundException.Message;
                statusCode = HttpStatusCode.BadRequest.ToString();
                break;
            case CheesyMartSystemValidationException cheesyMartSystemValidationException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                message = cheesyMartSystemValidationException.Message;
                statusCode = HttpStatusCode.BadRequest.ToString();
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                message = "An unexpected error occured in the system.We apologise for the inconvenience.";
                statusCode = HttpStatusCode.InternalServerError.ToString();
                break;
        }

        var errorResponse = new ErrorEventModel
        {
            EventId = Guid.NewGuid().ToString(),
            Message = message,
            StatusCode = statusCode
        };
        
        logger.LogError(exception, "Error with ID {EventId} in CheesyMart " +
                                   "API with body {@errorResponse}",errorResponse.EventId, errorResponse);

        await context.Response.WriteAsJsonAsync(errorResponse, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });
    }
}