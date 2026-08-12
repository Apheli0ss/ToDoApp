namespace TodoApp.Domain.Entities;

public class SubtaskItem
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public bool Completed { get; private set; }
    public int OrderNo { get; private set; }
    
    // İlişki (Foreign Key)
    public int TodoItemId { get; private set; }
    public TodoItem TodoItem { get; private set; } = null!;

    public SubtaskItem(string title, bool completed = false, int orderNo = 0)
    {
        Title = title;
        Completed = completed;
        OrderNo = orderNo;
    }

    public void UpdateStatus(bool completed)
    {
        Completed = completed;
    }

    public void UpdateTitle(string title)
    {
        Title = title;
    }
}
