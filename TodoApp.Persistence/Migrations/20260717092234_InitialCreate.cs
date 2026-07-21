// DateTime gibi temel .NET sınıflarını kullanabilmek için eklenir.
using System;

// EF Core Migration sınıflarını kullanabilmek için eklenir.
using Microsoft.EntityFrameworkCore.Migrations;

// Nullable Reference Types kapatılmış.
// Bu dosya EF Core tarafından üretildiği için
// uyarı vermemesi amacıyla kullanılır.
#nullable disable

// Migration dosyasının bulunduğu namespace.
namespace TodoApp.Persistence.Migrations
{
    /// <summary>
    /// İlk migration.
    ///
    /// Migration nedir?
    /// Entity sınıfları ile veritabanı arasındaki değişiklikleri tutan dosyadır.
    ///
    /// Bu dosya sayesinde EF Core,
    /// SQL Server'da hangi tabloların oluşturulacağını bilir.
    /// </summary>
    public partial class InitialCreate : Migration
    {
        /// <summary>
        /// Migration uygulanırken çalışan metot.
        ///
        /// Update-Database komutu çalıştırıldığında
        /// bu metot çalışır.
        ///
        /// Görevi:
        /// Veritabanına yeni tablolar oluşturmak.
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // TodoItems isimli tablo oluşturuluyor.
            migrationBuilder.CreateTable(

                // Tablo adı
                name: "TodoItems",

                // Kolonlar oluşturuluyor.
                columns: table => new
                {
                    // Primary Key
                    //
                    // int
                    // Identity(1,1)
                    //
                    // Identity:
                    // İlk kayıt 1
                    // Sonraki kayıtlar +1 artar.
                    Id = table.Column<int>(
                        type: "int",
                        nullable: false)

                        .Annotation("SqlServer:Identity", "1, 1"),

                    // Başlık
                    //
                    // string
                    //
                    // nullable olmadığı için
                    // NOT NULL oluşur.
                    Title = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: false),

                    // Açıklama
                    //
                    // Nullable olduğu için
                    // NULL olabilir.
                    Description = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: true),

                    // Tamamlandı mı?
                    //
                    // bool
                    //
                    // SQL'de BIT olur.
                    IsCompleted = table.Column<bool>(
                        type: "bit",
                        nullable: false),

                    // Priority enum
                    //
                    // EF Core enum'ı
                    // int olarak saklar.
                    Priority = table.Column<int>(
                        type: "int",
                        nullable: false),

                    // Oluşturulma tarihi
                    CreatedDate = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: false),

                    // Son teslim tarihi
                    //
                    // Nullable olduğu için
                    // NULL olabilir.
                    DueDate = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: true)
                },

                // Tabloya ait kısıtlamalar (Constraints)
                constraints: table =>
                {
                    // Primary Key oluşturuyor.
                    //
                    // PK_TodoItems
                    //
                    // Id alanı Primary Key oluyor.
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                });
        }

        /// <summary>
        /// Migration geri alınırken çalışan metot.
        ///
        /// Remove-Migration
        /// veya
        /// Update-Database EskiMigration
        ///
        /// gibi işlemlerde çalışır.
        ///
        /// Görevi:
        /// Up() metodunun yaptığı işlemleri geri almaktır.
        /// </summary>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // TodoItems tablosunu siliyor.
            migrationBuilder.DropTable(
                name: "TodoItems");
        }
    }
}