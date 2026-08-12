using FluentValidation;
using MediatR;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.DTOs;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Enums;

namespace TodoApp.Application.Features.Rooms.Commands.JoinRoom;

public sealed record JoinRoomCommand(int UserId, JoinRoomDto Dto) : IRequest<RoomDto>;

public sealed class JoinRoomCommandValidator : AbstractValidator<JoinRoomCommand>
{
    public JoinRoomCommandValidator()
    {
        RuleFor(x => x.Dto.RoomCode)
            .NotEmpty().WithMessage("Oda kodu boş olamaz.")
            .Length(6).WithMessage("Oda kodu 6 karakter olmalıdır.");
    }
}

public sealed class JoinRoomCommandHandler : IRequestHandler<JoinRoomCommand, RoomDto>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomMemberRepository _roomMemberRepository;

    public JoinRoomCommandHandler(IRoomRepository roomRepository, IRoomMemberRepository roomMemberRepository)
    {
        _roomRepository = roomRepository;
        _roomMemberRepository = roomMemberRepository;
    }

    public async Task<RoomDto> Handle(JoinRoomCommand request, CancellationToken cancellationToken)
    {
        var roomCode = request.Dto.RoomCode?.Trim().ToUpperInvariant() ?? string.Empty;

        var room = await _roomRepository.GetByCodeAsync(roomCode, cancellationToken);
        if (room == null)
            throw new NotFoundException("Belirtilen kod ile bir oda bulunamadı.");

        // Check if user is already in the room
        var isMember = await _roomMemberRepository.IsMemberAsync(room.Id, request.UserId, cancellationToken);
        if (!isMember)
        {
            var roomMember = new RoomMember(room.Id, request.UserId, Role.Member);
            await _roomMemberRepository.AddAsync(roomMember, cancellationToken);
            await _roomRepository.SaveChangesAsync(cancellationToken);
        }

        return new RoomDto 
        { 
            Id = room.Id,
            RoomCode = room.RoomCode,
            Name = room.Name,
            CreatedByUserId = room.CreatedByUserId,
            CreatedAt = room.CreatedAt
        };
    }
}
