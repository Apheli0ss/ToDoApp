using TodoApp.Domain.Entities;

namespace TodoApp.Application.Abstractions.Persistence;

public interface IRoomRepository
{
    Task<Room?> GetByCodeAsync(string roomCode, CancellationToken cancellationToken);
    Task<Room?> GetByIdAsync(int roomId, CancellationToken cancellationToken);
    Task AddAsync(Room room, CancellationToken cancellationToken);
    void Remove(Room room);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
