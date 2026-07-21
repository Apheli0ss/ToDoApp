namespace TodoApp.Domain.Enums;

/// <summary>
/// Todo önceliğinin desteklenen değerleri.
/// </summary>
public enum Priority
{
    Low = 1,
    Medium = 2,
    High = 3
}

public enum TodoStatus
{
    Pending = 0,      // Bekliyor
    InProgress = 1,   // Devam Ediyor
    Completed = 2     // Tamamlandı
}