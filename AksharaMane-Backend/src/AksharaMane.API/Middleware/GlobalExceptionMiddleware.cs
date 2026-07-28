
using AksharaMane.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace AksharaMane.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(
                context,
                exception);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var statusCode = exception switch
        {
            BadRequestException =>
                HttpStatusCode.BadRequest,

            UnauthorizedException =>
                HttpStatusCode.Unauthorized,

            NotFoundException =>
                HttpStatusCode.NotFound,

            ConflictException =>
                HttpStatusCode.Conflict,

            _ =>
                HttpStatusCode.InternalServerError
        };

        if (statusCode ==
            HttpStatusCode.InternalServerError)
        {
            _logger.LogError(
                exception,
                "Unhandled exception for {Method} {Path}",
                context.Request.Method,
                context.Request.Path);
        }
        else
        {
            _logger.LogWarning(
                exception,
                "Request failed with status {StatusCode}",
                (int)statusCode);
        }

        var detail =
            statusCode == HttpStatusCode.InternalServerError &&
            !_environment.IsDevelopment()
                ? "An unexpected error occurred."
                : exception.Message;

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = GetTitle(statusCode),
            Detail = detail,
            Instance = context.Request.Path
        };

        problemDetails.Extensions["traceId"] =
            context.TraceIdentifier;

        context.Response.StatusCode =
            problemDetails.Status.Value;

        context.Response.ContentType =
            "application/problem+json";

        var json = JsonSerializer.Serialize(
            problemDetails,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy =
                    JsonNamingPolicy.CamelCase
            });

        await context.Response.WriteAsync(json);
    }

    private static string GetTitle(
        HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            HttpStatusCode.BadRequest =>
                "Invalid request",

            HttpStatusCode.Unauthorized =>
                "Authentication failed",

            HttpStatusCode.NotFound =>
                "Resource not found",

            HttpStatusCode.Conflict =>
                "Request conflict",

            _ =>
                "Server error"
        };
    }
}