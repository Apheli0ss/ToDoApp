using FluentValidation;
using MediatR;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Application.Abstractions.Services;
using TodoApp.Application.DTOs;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Enums;

namespace TodoApp.Application.Features.Rooms.Commands.CreateRoom;

public sealed record CreateRoomCommand(int UserId, CreateRoomDto Dto) : IRequest<RoomDto>;

public sealed class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotEmpty().WithMessage("Oda adı boş olamaz.")
            .MaximumLength(100).WithMessage("Oda adı en fazla 100 karakter olabilir.");
    }
}

public sealed class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomDto>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomCodeGenerator _roomCodeGenerator;

    public CreateRoomCommandHandler(IRoomRepository roomRepository, IRoomCodeGenerator roomCodeGenerator)
    {
        _roomRepository = roomRepository;
        _roomCodeGenerator = roomCodeGenerator;
    }

    public async Task<RoomDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var code = await _roomCodeGenerator.GenerateUniqueCodeAsync(cancellationToken);
        
        var room = new Room(code, request.Dto.Name, request.UserId);
        var roomMember = new RoomMember(room.Id, request.UserId, Role.Owner);
        
        room.RoomMembers.Add(roomMember);
        
        await _roomRepository.AddAsync(room, cancellationToken);
        await _roomRepository.SaveChangesAsync(cancellationToken);
        
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
