using FluentValidation;
using MediatR;

namespace TodoApp.Application.Common.Behaviors;

/// <summary>
/// Command ve query çalışmadan önce FluentValidation kurallarını uygular.
/// Böylece handler'larda tekrar eden doğrulama kodu oluşmaz (SRP/DRY).
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Tüm validator sonuçlarını birleştirir; hata varsa handler'a geçmeden exception fırlatır.
    /// </summary>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Validator tanımlı değilse request doğrudan bir sonraki adıma geçer.
        if (!validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .ToList();

        // FluentValidation'ın exception türü global middleware tarafından HTTP 400'e çevrilir.
        if (failures.Count != 0)
            throw new ValidationException(failures);

        return await next(cancellationToken);
    }
}
