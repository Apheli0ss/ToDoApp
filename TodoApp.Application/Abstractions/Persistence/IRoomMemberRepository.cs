using TodoApp.Domain.Entities;

namespace TodoApp.Application.Abstractions.Persistence;

public interface IRoomMemberRepository
{
    Task<RoomMember?> GetAsync(int roomId, int userId, CancellationToken cancellationToken);
    Task AddAsync(RoomMember roomMember, CancellationToken cancellationToken);
    void Remove(RoomMember roomMember);
    Task<bool> IsMemberAsync(int roomId, int userId, CancellationToken cancellationToken);
}
