using TodoApp.Domain.Enums;

namespace TodoApp.Domain.Entities;

/// <summary>Veritabanındaki TodoItems kaydını temsil eder.</summary>
public class TodoItem
{
    public int Id { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public TodoStatus Status { get; private set; } = TodoStatus.Pending;

    public Priority Priority { get; private set; }

    public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; private set; }

    // EF Core private yapıcıyı veritabanından nesne oluştururken kullanabilir.
    private TodoItem()
    {
    }

    public TodoItem(string title, string? description, Priority priority, DateTime? dueDate)
    {
        UpdateDetails(title, description, priority, dueDate);
        CreatedDate = DateTime.UtcNow;
        Status = TodoStatus.Pending;
    }

    /// <summary>Yalnızca düzenlenebilir alanları tek bir domain işlemiyle değiştirir.</summary>
    public void UpdateDetails(string title, string? description, Priority priority, DateTime? dueDate)
    {
        Title = title.Trim();
        Description = description?.Trim();
        Priority = priority;
        DueDate = dueDate;
    }

    /// <summary>Durum değişimini entity üzerinden kontrollü olarak uygular.</summary>
    public void SetStatus(TodoStatus status) => Status = status;
}
