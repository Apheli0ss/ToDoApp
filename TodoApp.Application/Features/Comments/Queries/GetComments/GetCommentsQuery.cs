using MediatR;
using TodoApp.Application.Abstractions.Persistence;

namespace TodoApp.Application.Features.Comments.Queries.GetComments;

public sealed record CommentDto(
    int Id,
    int TodoItemId,
    int UserId,
    string UserName,
    string Content,
    DateTime CreatedAt
);

public sealed record GetCommentsQuery(int TodoItemId) : IRequest<List<CommentDto>>;

public sealed class GetCommentsQueryHandler(ICommentRepository commentRepository) : IRequestHandler<GetCommentsQuery, List<CommentDto>>
{
    public async Task<List<CommentDto>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        var comments = await commentRepository.GetByTodoItemIdAsync(request.TodoItemId, cancellationToken);
        return comments.Select(c => new CommentDto(
            c.Id,
            c.TodoItemId,
            c.UserId,
            c.User?.Username ?? "Unknown",
            c.Content,
            c.CreatedAt
        )).ToList();
    }
}
