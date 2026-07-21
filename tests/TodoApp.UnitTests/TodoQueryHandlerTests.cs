using Moq;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Application.Features.Todos.Queries.GetTodoById;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Enums;

namespace TodoApp.UnitTests;

public class TodoQueryHandlerTests
{
    [Fact]
    public async Task Handle_GecerliId_Verildiginde_TodoDonmeli()
    {
        var repository = new Mock<ITodoRepository>();
        repository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TodoItem("ASP.NET Core Öğren", null, Priority.High, null));

        var handler = new GetTodoByIdQueryHandler(repository.Object);
        var result = await handler.Handle(new GetTodoByIdQuery(1), CancellationToken.None);

        Assert.Equal("ASP.NET Core Öğren", result.Title);
        Assert.Equal(Priority.High, result.Priority);
    }
}
