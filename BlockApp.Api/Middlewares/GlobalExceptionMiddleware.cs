
using System.Net;
using System.Text.Json;
using BlockApp.Api.Exceptions;
using BlockApp.Shared.DTOs;

namespace BlockApp.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, errorCode, message) = MapException(ex);

        context.Response.StatusCode = (int)statusCode;

        // Log เฉพาะฝั่ง server
        _logger.LogError(ex, "Unhandled exception");

        var response = new ErrorResponse
        {
            Error = errorCode,
            Message = message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response)
        );
    }

    private static (HttpStatusCode, string, string) MapException(Exception ex)
    {
        return ex switch
        {
            RateLimitException => (
                HttpStatusCode.TooManyRequests,
                "RATE_LIMIT",
                ex.Message
            ),

            InvalidOtpException => (
                HttpStatusCode.BadRequest,
                "INVALID_OTP",
                ex.Message
            ),

            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                "UNAUTHORIZED",
                "Authentication required"
            ),

            _ => (
                HttpStatusCode.InternalServerError,
                "SERVER_ERROR",
                "Unexpected error occurred"
            )
        };
    }
}
