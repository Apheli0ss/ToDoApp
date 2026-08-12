using MediatR;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.DTOs;

namespace TodoApp.Application.Features.Rooms.Queries.GetRoomByCode;

public sealed record GetRoomByCodeQuery(int UserId, string RoomCode) : IRequest<RoomDto>;

public sealed class GetRoomByCodeQueryHandler(IRoomRepository roomRepository)
    : IRequestHandler<GetRoomByCodeQuery, RoomDto>
{
    public async Task<RoomDto> Handle(GetRoomByCodeQuery request, CancellationToken cancellationToken)
    {
        // Membership is authorization data, so it must never be served from a stale cache.
        var room = await roomRepository.GetByCodeAsync(request.RoomCode, cancellationToken)
            ?? throw new NotFoundException("Oda bulunamadı.");

        if (!room.RoomMembers.Any(member => member.UserId == request.UserId))
            throw new UnauthorizedAccessException("Bu odayı görüntüleme yetkiniz yok.");

        return new RoomDto
        {
            Id = room.Id,
            RoomCode = room.RoomCode,
            Name = room.Name,
            CreatedByUserId = room.CreatedByUserId,
            CreatedAt = room.CreatedAt,
            Members = room.RoomMembers.Select(member => new RoomMemberDto
            {
                UserId = member.UserId,
                Username = member.User?.Username ?? string.Empty,
                Email = member.User?.Email ?? string.Empty,
                Role = member.Role,
                JoinedAt = member.JoinedAt
            }).ToList()
        };
    }
}
