namespace TodoApp.Domain.Entities;

public class Notification
{
    public int Id { get; private set; }
    
    public int UserId { get; private set; }
    public User User { get; private set; } = null!;
    
    public string Message { get; private set; } = string.Empty;
    public bool IsRead { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Notification(int userId, string message)
    {
        UserId = userId;
        Message = message;
        IsRead = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsRead()
    {
        IsRead = true;
    }

    protected Notification() { }
}
