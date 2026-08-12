namespace TodoApp.Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    
    public int TodoItemId { get; set; }
    public TodoItem? TodoItem { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
