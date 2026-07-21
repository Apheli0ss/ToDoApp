// Entity Framework Core kütüphanesi.
// ToListAsync(), FirstOrDefaultAsync() gibi metotlar burada bulunur.
using Microsoft.EntityFrameworkCore;

// Repository interface'ini kullanabilmek için ekleniyor.
using TodoApp.Application.Abstractions.Persistence;

// TodoItem Entity'sini kullanabilmek için ekleniyor.
using TodoApp.Domain.Entities;

// Repository sınıfının bulunduğu namespace.
namespace TodoApp.Persistence.Repositories;

/// <summary>
/// Todo verileri üzerinde CRUD işlemlerini yapan Repository sınıfı.
///
/// Repository'nin görevi:
/// - Veritabanından veri okumak
/// - Veri eklemek
/// - Veri güncellemek
/// - Veri silmek
///
/// Dikkat:
/// Burada iş kuralları yazılmaz.
/// Sadece veri erişimi yapılır.
/// </summary>
public class TodoRepository : ITodoRepository
{
    /// <summary>
    /// EF Core'un DbContext nesnesi.
    ///
    /// DbContext sayesinde
    /// SQL Server ile iletişim kurulur.
    /// </summary>
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor
    ///
    /// Dependency Injection sistemi
    /// AppDbContext'i otomatik olarak gönderir.
    /// </summary>
    public TodoRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Bütün Todo kayıtlarını getirir.
    /// </summary>
    public async Task<List<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // TodoItems tablosunu al.
        //
        // OrderByDescending:
        // En yeni kayıtlar üstte olacak şekilde sırala.
        //
        // SQL:
        // ORDER BY CreatedDate DESC
        //
        // ToListAsync():
        // Sorguyu çalıştır ve sonucu liste olarak getir.
        return await _context.TodoItems
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Id'ye göre tek bir Todo getirir.
    /// </summary>
    public async Task<TodoItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        // İlk eşleşen kaydı getir.
        //
        // SQL:
        // SELECT TOP(1)
        // FROM TodoItems
        // WHERE Id=@id
        return await _context.TodoItems
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <summary>
    /// Yeni Todo ekler.
    /// </summary>
    public async Task AddAsync(TodoItem todo, CancellationToken cancellationToken = default)
    {
        // Todo nesnesini EF Core'un Change Tracker'ına ekler.
        //
        // Dikkat:
        // Bu satır INSERT çalıştırmaz.
        //
        // INSERT,
        // SaveChangesAsync() sırasında çalışır.
        await _context.TodoItems.AddAsync(todo, cancellationToken);
    }

    /// <summary>
    /// Todo günceller.
    /// </summary>
    public Task UpdateAsync(TodoItem todo, CancellationToken cancellationToken = default)
    {
        // EF Core'a
        // "Bu nesne güncellendi." bilgisini verir.
        //
        // SQL henüz çalışmaz.
        _context.TodoItems.Update(todo);

        // Update() metodu senkron çalıştığı için
        // tamamlanmış bir Task döndürülür.
        return Task.CompletedTask;
    }

    /// <summary>
    /// Todo siler.
    /// </summary>
    public Task DeleteAsync(TodoItem todo, CancellationToken cancellationToken = default)
    {
        // EF Core'a
        // "Bu nesne silinecek." bilgisini verir.
        //
        // DELETE sorgusu henüz çalışmaz.
        _context.TodoItems.Remove(todo);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Yapılan tüm değişiklikleri veritabanına kaydeder.
    ///
    /// INSERT
    /// UPDATE
    /// DELETE
    ///
    /// işlemleri burada SQL'e dönüşür.
    /// </summary>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // EF Core'un takip ettiği tüm değişiklikleri
        // SQL Server'a gönderir.
        await _context.SaveChangesAsync(cancellationToken);
    }
}
