using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Common.Behaviors;

namespace TodoApp.Application;

/// <summary>
/// Application katmanının DI kayıtlarını tek bir noktada toplar.
/// API katmanı handler veya validator sınıflarını bilmez; yalnızca bu genişletmeyi çağırır.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// MediatR handler'larını, FluentValidation validator'larını ve pipeline davranışlarını kaydeder.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Handler taraması Application assembly'si ile sınırlıdır.
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // Her request için ilgili validator otomatik bulunur.
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Her MediatR isteğinden önce doğrulama uygulanır.
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
