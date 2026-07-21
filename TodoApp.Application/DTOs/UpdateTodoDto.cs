// Domain katmanındaki Priority enum'ını kullanabilmek için eklenir.
// Örneğin: Low, Medium, High
using TodoApp.Domain.Enums;

// Bu DTO'nun Application katmanında bulunduğunu belirtir.
namespace TodoApp.Application.DTOs;

/// <summary>
/// Mevcut bir Todo güncellenirken istemciden (React, Postman vb.)
/// gelecek verileri temsil eden DTO (Data Transfer Object) sınıfıdır.
///
/// DTO kullanılmasının amacı:
/// - Entity'yi doğrudan dış dünyaya açmamak
/// - Sadece güncellenmesi gereken alanları almak
/// - Katmanlar arasında veri taşımak
/// </summary>
public class UpdateTodoDto
{
    /// <summary>
    /// Güncellenecek Todo'nun yeni başlığı.
    ///
    /// string.Empty verilmesinin sebebi
    /// property'sinin başlangıçta null olmamasını sağlamaktır.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Todo açıklaması.
    ///
    /// Nullable (?) olduğu için kullanıcı isterse boş bırakabilir.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Todo'nun yeni öncelik seviyesi.
    ///
    /// Enum kullanıldığı için yalnızca
    /// tanımlanmış değerlerden biri seçilebilir.
    /// (Low, Medium, High vb.)
    /// </summary>
    public Priority Priority { get; set; }

    /// <summary>
    /// Todo'nun yeni son teslim tarihi.
    ///
    /// Nullable olduğu için tarih verilmesi zorunlu değildir.
    /// </summary>
    public DateTime? DueDate { get; set; }
}