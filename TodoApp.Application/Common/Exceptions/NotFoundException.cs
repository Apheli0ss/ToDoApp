namespace TodoApp.Application.Common.Exceptions;

/// <summary>
/// İstenen kayıt bulunamadığında fırlatılır.
///
/// API katmanındaki global exception handling middleware'i bu exception'ı
/// yakalayıp otomatik olarak HTTP 404 (Not Found) durumuna çevirir.
/// Bu sayede Service katmanı hiçbir HTTP detayı bilmek zorunda kalmaz.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string entityName, object key)
        : base($"'{entityName}' ({key}) bulunamadı.")
    {
    }
}
