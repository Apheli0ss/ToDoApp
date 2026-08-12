namespace TodoApp.Application.Abstractions.Services;

public interface IRoomCodeGenerator
{
    Task<string> GenerateUniqueCodeAsync(CancellationToken cancellationToken);
}
