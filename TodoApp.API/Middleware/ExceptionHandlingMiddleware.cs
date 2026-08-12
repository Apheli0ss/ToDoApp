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
            // Istegi zincirdeki bir sonraki middleware veya controller'a iletir.
            await next(context);
        }
        catch (Exception exception)
        {
            // Uygulama hatalarinin kontrolsuz bicimde istemciye yayilmasini engeller.
            await WriteErrorAsync(context, exception);
        }
    }

    private async Task WriteErrorAsync(HttpContext context, Exception exception)
    {
        // Hata turu, istemcinin anlayacagi HTTP kodu ve basliga cevrilir.
        var (status, title) = exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, "Kayıt bulunamadı."),
            ValidationException or ArgumentException => (HttpStatusCode.BadRequest, "Geçersiz istek."),
            UnauthorizedAccessException => (HttpStatusCode.Forbidden, "Bu işlem için yetkiniz yok."),
            OperationCanceledException => ((HttpStatusCode)499, "İstek iptal edildi."),
            _ => (HttpStatusCode.InternalServerError, "Sunucu hatası.")
        };

        // Beklenmeyen sunucu hatasi sadece sunucu tarafinda ayrintili loglanir.
        if (status == HttpStatusCode.InternalServerError)
            logger.LogError(exception, "Beklenmeyen hata. Path: {Path}", context.Request.Path);

        // Production ortaminda 500 hatasinin teknik ayrintisi guvenlik nedeniyle gizlenir.
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

        // ProblemDetails standart hata biciminde JSON cevabi yazilir.
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
