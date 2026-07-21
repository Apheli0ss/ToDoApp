// Entity Framework Core'u kullanabilmek için ekleniyor.
//
// DbContext
// DbSet
// DbContextOptions
//
// gibi sınıflar burada bulunur.
using Microsoft.EntityFrameworkCore;

// TodoItem Entity'sini kullanabilmek için ekleniyor.
using TodoApp.Domain.Entities;

// Bu sınıfın Persistence katmanında olduğunu belirtir.
namespace TodoApp.Persistence;

/// <summary>
/// Uygulamanın veritabanı ile iletişim kurduğu ana sınıf.
///
/// DbContext nedir?
///
/// EF Core'un merkezidir.
///
/// Görevleri:
/// - SQL Server bağlantısını yönetmek
/// - Entity'leri tanımak
/// - SQL üretmek
/// - Değişiklikleri takip etmek (Change Tracker)
/// - Migration oluşturmak
/// - SaveChanges işlemini yapmak
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Constructor
    ///
    /// ASP.NET Core Dependency Injection sistemi
    /// DbContextOptions nesnesini otomatik gönderir.
    ///
    /// Bu options nesnesi içinde:
    ///
    /// - Connection String
    /// - SQL Server sağlayıcısı
    /// - EF Core ayarları
    ///
    /// bulunur.
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// TodoItems tablosunu temsil eder.
    ///
    /// DbSet nedir?
    ///
    /// Veritabanındaki bir tabloyu temsil eden koleksiyondur.
    ///
    /// EF Core bu satırı görünce
    /// TodoItems isminde tablo oluşturur.
    ///
    /// Repository bu DbSet üzerinden
    /// CRUD işlemlerini yapar.
    /// </summary>
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
}