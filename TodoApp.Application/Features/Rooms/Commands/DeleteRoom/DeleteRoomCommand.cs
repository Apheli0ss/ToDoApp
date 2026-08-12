using MediatR;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Domain.Enums;

namespace TodoApp.Application.Features.Rooms.Commands.DeleteRoom;

public sealed record DeleteRoomCommand(int UserId, string RoomCode) : IRequest;

public sealed class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomMemberRepository _roomMemberRepository;

    public DeleteRoomCommandHandler(IRoomRepository roomRepository, IRoomMemberRepository roomMemberRepository)
    {
        _roomRepository = roomRepository;
        _roomMemberRepository = roomMemberRepository;
    }

    public async Task Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByCodeAsync(request.RoomCode, cancellationToken);
        if (room == null)
            throw new NotFoundException("Oda bulunamadı.");

        // Yalnızca Owner yetkisine sahip kişi odayı silebilir.
        var member = await _roomMemberRepository.GetAsync(room.Id, request.UserId, cancellationToken);
        if (member == null || member.Role != Role.Owner)
            throw new UnauthorizedAccessException("Odayı silmek için 'Owner' yetkisine sahip olmalısınız.");

        // Odadaki todos ve member kayıtları Cascade silinme kuralları gereği otomatik silinir. (DbContext konfigürasyonu)
        _roomRepository.Remove(room);
        await _roomRepository.SaveChangesAsync(cancellationToken);
    }
}
