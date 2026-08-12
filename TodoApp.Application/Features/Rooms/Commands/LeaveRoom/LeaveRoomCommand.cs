using MediatR;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Application.Common.Exceptions;

namespace TodoApp.Application.Features.Rooms.Commands.LeaveRoom;

public sealed record LeaveRoomCommand(int UserId, string RoomCode) : IRequest;

public sealed class LeaveRoomCommandHandler : IRequestHandler<LeaveRoomCommand>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomMemberRepository _roomMemberRepository;

    public LeaveRoomCommandHandler(IRoomRepository roomRepository, IRoomMemberRepository roomMemberRepository)
    {
        _roomRepository = roomRepository;
        _roomMemberRepository = roomMemberRepository;
    }

    public async Task Handle(LeaveRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByCodeAsync(request.RoomCode, cancellationToken);
        if (room == null)
            throw new NotFoundException("Oda bulunamadı.");

        var member = await _roomMemberRepository.GetAsync(room.Id, request.UserId, cancellationToken);
        if (member == null)
            throw new BadRequestException("Bu odanın bir üyesi değilsiniz.");

        _roomMemberRepository.Remove(member);
        await _roomRepository.SaveChangesAsync(cancellationToken);
    }
}
