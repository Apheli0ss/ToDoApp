namespace TodoApp.Application.DTOs;

public class SubtaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; }
}
