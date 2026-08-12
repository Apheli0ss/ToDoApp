// Priority enum'ını kullanabilmek için Domain katmanını projeye dahil ediyoruz.
// Örneğin: Low, Medium, High gibi öncelik değerleri burada tanımlıdır.
using TodoApp.Domain.Enums;

// Bu sınıfın Application katmanındaki DTOs klasöründe bulunduğunu belirtir.
namespace TodoApp.Application.DTOs;

/// <summary>
/// Yeni bir Todo oluşturulurken istemciden (React, Postman vb.)
/// gelecek verileri temsil eden DTO (Data Transfer Object) sınıfıdır.
///
/// DTO'nun amacı:
/// - Entity'yi dış dünyaya açmamak
/// - Gereksiz alanların gönderilmesini engellemek
/// - Veri transferini güvenli ve düzenli hale getirmek
/// </summary>
public class CreateTodoDto
{
    /// <summary>
    /// Yapılacak görevin başlığı.
    /// Kullanıcının doldurması beklenir.
    ///
    /// "= string.Empty" sayesinde null olmaz.
    /// Böylece NullReferenceException riski azalır.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Görev açıklaması.
    ///
    /// Sonundaki ? işareti bu alanın nullable olduğunu gösterir.
    /// Kullanıcı isterse boş bırakabilir.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Görevin öncelik seviyesi.
    ///
    /// Enum kullanılmasının sebebi;
    /// kullanıcı yalnızca belirlenen değerleri seçebilir.
    ///
    /// Örneğin:
    /// Low
    /// Medium
    /// High
    /// </summary>
    public Priority Priority { get; set; }

    /// <summary>
    /// Görevin son teslim tarihi.
    ///
    /// Nullable DateTime olduğu için
    /// kullanıcı tarih seçmeyebilir.
    /// </summary>
    public DateTime? DueDate { get; set; }

    public string? Assignee { get; set; }
    
    public string? Category { get; set; }
    
    public string? EstimatedTime { get; set; }
    
    public List<SubtaskDto>? Subtasks { get; set; } = new();
    
    public int? RoomId { get; set; }
}