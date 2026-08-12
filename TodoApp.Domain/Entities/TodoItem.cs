using TodoApp.Domain.Enums;

namespace TodoApp.Domain.Entities;

/// <summary>Veritabanındaki TodoItems kaydını temsil eder.</summary>
public class TodoItem
{
    public int Id { get; private set; }
    
    public int UserId { get; private set; }
    public User User { get; private set; } = null!;

    public int? RoomId { get; private set; }
    public Room? Room { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public TodoStatus Status { get; private set; } = TodoStatus.Pending;

    public Priority Priority { get; private set; }

    public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; private set; }

    public string? Assignee { get; private set; }
    
    public string? Category { get; private set; }
    
    public string? EstimatedTime { get; private set; }

    private readonly List<SubtaskItem> _subtasks = new();
    public IReadOnlyCollection<SubtaskItem> Subtasks => _subtasks.AsReadOnly();


    public TodoItem(int userId, string title, string? description, Priority priority, DateTime? dueDate, string? assignee = null, string? category = null, string? estimatedTime = null, int? roomId = null)
    {
        UserId = userId;
        RoomId = roomId;
        UpdateDetails(title, description, priority, dueDate, assignee, category, estimatedTime);
        CreatedDate = DateTime.UtcNow;
        Status = TodoStatus.Pending;
    }

    /// <summary>Yalnızca düzenlenebilir alanları tek bir domain işlemiyle değiştirir.</summary>
    public void UpdateDetails(string title, string? description, Priority priority, DateTime? dueDate, string? assignee = null, string? category = null, string? estimatedTime = null)
    {
        Title = title.Trim();
        Description = description?.Trim();
        Priority = priority;
        DueDate = dueDate;
        Assignee = assignee?.Trim();
        Category = category?.Trim();
        EstimatedTime = estimatedTime?.Trim();
    }

    public void SetStatus(TodoStatus status) => Status = status;

    public void UpdateSubtasks(IEnumerable<(string Title, bool Completed)> subtasks)
    {
        _subtasks.Clear();
        if (subtasks != null)
        {
            foreach (var st in subtasks)
            {
                _subtasks.Add(new SubtaskItem(st.Title, st.Completed));
            }
        }
    }
}
