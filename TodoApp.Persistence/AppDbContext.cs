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

    public DbSet<SubtaskItem> Subtasks => Set<SubtaskItem>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomMember> RoomMembers => Set<RoomMember>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // TodoItem Settings
        modelBuilder.Entity<TodoItem>()
            .Property(t => t.Title)
            .HasMaxLength(200);
            
        modelBuilder.Entity<TodoItem>()
            .Property(t => t.Description)
            .HasMaxLength(2000);

        modelBuilder.Entity<TodoItem>()
            .Property(t => t.Category)
            .HasMaxLength(50);
            
        modelBuilder.Entity<TodoItem>()
            .Property(t => t.EstimatedTime)
            .HasMaxLength(50);
            
        modelBuilder.Entity<TodoItem>()
            .Property(t => t.Assignee)
            .HasMaxLength(50);

        modelBuilder.Entity<TodoItem>()
            .HasIndex(t => t.Status);

        modelBuilder.Entity<TodoItem>()
            .HasIndex(t => t.Priority);

        modelBuilder.Entity<TodoItem>()
            .HasMany(t => t.Subtasks)
            .WithOne(s => s.TodoItem)
            .HasForeignKey(s => s.TodoItemId)
            .OnDelete(DeleteBehavior.Cascade);

        // User Settings
        modelBuilder.Entity<User>()
            .Property(u => u.Username)
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasMaxLength(256);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasMany(u => u.Todos)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Room Settings
        modelBuilder.Entity<Room>()
            .Property(r => r.RoomCode)
            .HasMaxLength(6)
            .HasColumnType("varchar(6)");
            
        modelBuilder.Entity<Room>()
            .Property(r => r.Name)
            .HasMaxLength(100);

        modelBuilder.Entity<Room>()
            .HasIndex(r => r.RoomCode)
            .IsUnique();

        // RoomMember Configurations
        modelBuilder.Entity<RoomMember>(entity =>
        {
            entity.HasKey(rm => new { rm.RoomId, rm.UserId });
            entity.HasOne(rm => rm.Room)
                  .WithMany(r => r.RoomMembers)
                  .HasForeignKey(rm => rm.RoomId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(rm => rm.User)
                  .WithMany(u => u.RoomMembers)
                  .HasForeignKey(rm => rm.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Comment Configurations
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasOne(c => c.TodoItem)
                  .WithMany() 
                  .HasForeignKey(c => c.TodoItemId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(c => c.User)
                  .WithMany()
                  .HasForeignKey(c => c.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.Property(c => c.Content).IsRequired();
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(n => n.Id);
            entity.Property(n => n.Message).IsRequired().HasMaxLength(500);

            entity.HasOne(n => n.User)
                  .WithMany()
                  .HasForeignKey(n => n.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TodoItem>()
            .HasOne(t => t.Room)
            .WithMany(r => r.Todos)
            .HasForeignKey(t => t.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}