using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Common.Exceptions;

namespace TodoApp.API.Middleware;

/// <summary>
/// Application hatalarını istemci için güvenli ProblemDetails cevaplarına dönüştürür.
/// </summary>
public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IHostEnvironment environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await WriteErrorAsync(context, exception);
        }
    }

    private async Task WriteErrorAsync(HttpContext context, Exception exception)
    {
        var (status, title) = exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, "Kayıt bulunamadı."),
            ValidationException or ArgumentException => (HttpStatusCode.BadRequest, "Geçersiz istek."),
            OperationCanceledException => ((HttpStatusCode)499, "İstek iptal edildi."),
            _ => (HttpStatusCode.InternalServerError, "Sunucu hatası.")
        };

        if (status == HttpStatusCode.InternalServerError)
            logger.LogError(exception, "Beklenmeyen hata. Path: {Path}", context.Request.Path);

        var detail = environment.IsDevelopment() || status != HttpStatusCode.InternalServerError
            ? exception.Message
            : "Sunucuda beklenmeyen bir hata oluştu.";
        var problem = new ProblemDetails
        {
            Status = (int)status,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
