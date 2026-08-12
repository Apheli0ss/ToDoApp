using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.Persistence.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly AppDbContext _context;

    public RoomRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Room?> GetByCodeAsync(string roomCode, CancellationToken cancellationToken)
    {
        return await _context.Rooms
            .Include(r => r.RoomMembers)
                .ThenInclude(rm => rm.User)
            .FirstOrDefaultAsync(r => r.RoomCode == roomCode, cancellationToken);
    }

    public async Task<Room?> GetByIdAsync(int roomId, CancellationToken cancellationToken)
    {
        return await _context.Rooms
            .Include(r => r.RoomMembers)
                .ThenInclude(rm => rm.User)
            .FirstOrDefaultAsync(r => r.Id == roomId, cancellationToken);
    }

    public async Task AddAsync(Room room, CancellationToken cancellationToken)
    {
        await _context.Rooms.AddAsync(room, cancellationToken);
    }

    public void Remove(Room room)
    {
        _context.Rooms.Remove(room);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
