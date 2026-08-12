using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.Persistence.Repositories;

public class RoomMemberRepository : IRoomMemberRepository
{
    private readonly AppDbContext _context;

    public RoomMemberRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<RoomMember?> GetAsync(int roomId, int userId, CancellationToken cancellationToken)
    {
        return _context.RoomMembers
            .FirstOrDefaultAsync(rm => rm.RoomId == roomId && rm.UserId == userId, cancellationToken);
    }

    public async Task AddAsync(RoomMember roomMember, CancellationToken cancellationToken)
    {
        await _context.RoomMembers.AddAsync(roomMember, cancellationToken);
    }

    public void Remove(RoomMember roomMember)
    {
        _context.RoomMembers.Remove(roomMember);
    }

    public Task<bool> IsMemberAsync(int roomId, int userId, CancellationToken cancellationToken)
    {
        return _context.RoomMembers
            .AnyAsync(rm => rm.RoomId == roomId && rm.UserId == userId, cancellationToken);
    }
}
