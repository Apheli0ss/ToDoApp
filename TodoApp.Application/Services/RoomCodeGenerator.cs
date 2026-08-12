using System.Security.Cryptography;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Application.Abstractions.Services;

namespace TodoApp.Application.Services;

public class RoomCodeGenerator : IRoomCodeGenerator
{
    private readonly IRoomRepository _roomRepository;

    public RoomCodeGenerator(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<string> GenerateUniqueCodeAsync(CancellationToken cancellationToken)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        
        while (true)
        {
            var randomBytes = new byte[6];
            RandomNumberGenerator.Fill(randomBytes);
            
            var codeArray = new char[6];
            for (int i = 0; i < 6; i++)
            {
                codeArray[i] = chars[randomBytes[i] % chars.Length];
            }
            var code = new string(codeArray);

            var existingRoom = await _roomRepository.GetByCodeAsync(code, cancellationToken);
            if (existingRoom == null)
            {
                return code;
            }
        }
    }
}
