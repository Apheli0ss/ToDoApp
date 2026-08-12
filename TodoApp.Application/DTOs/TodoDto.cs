// Domain katmanındaki Priority enum'ını kullanabilmek için ekleniyor.
// Örneğin: Low, Medium, High
using TodoApp.Domain.Enums;

// Bu sınıfın Application katmanındaki DTOs namespace'inde olduğunu belirtir.
namespace TodoApp.Application.DTOs;

/// <summary>
/// TodoDto, istemciye (React, Angular, Postman vb.)
/// geri gönderilecek Todo verisini temsil eder.
///
/// DTO (Data Transfer Object) olduğu için
/// veritabanındaki Entity'nin birebir kendisi değildir.
///
/// Amaç:
/// - Entity'yi dış dünyaya açmamak
/// - Gereken alanları istemciye göndermek
/// - Katmanlar arasında veri taşımak
/// </summary>
public class TodoDto
{
    /// <summary>
    /// Todo'nun veritabanındaki benzersiz kimliği (Primary Key).
    /// SQL Server'da genellikle Identity olarak otomatik oluşturulur.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Todo başlığı.
    ///
    /// Varsayılan olarak boş string atanır.
    /// Böylece null olmasının önüne geçilir.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Todo açıklaması.
    ///
    /// Nullable (?) olduğu için boş olabilir.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Görev tamamlandı mı?
    ///
    /// false -> Devam ediyor
    /// true  -> Tamamlandı
    /// </summary>
    public TodoStatus Status { get; set; }

    /// <summary>
    /// Görevin öncelik seviyesi.
    ///
    /// Enum kullanıldığı için
    /// sadece belirlenen değerleri alabilir.
    /// </summary>
    public Priority Priority { get; set; }

    /// <summary>
    /// Görevin oluşturulma tarihi.
    ///
    /// Service katmanında
    /// DateTime.UtcNow ile atanır.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Son teslim tarihi.
    ///
    /// Nullable olduğu için boş bırakılabilir.
    /// </summary>
    public DateTime? DueDate { get; set; }

    public string? Assignee { get; set; }
    
    public string? Category { get; set; }
    
    public string? EstimatedTime { get; set; }
    
    public List<SubtaskDto> Subtasks { get; set; } = new();
}
