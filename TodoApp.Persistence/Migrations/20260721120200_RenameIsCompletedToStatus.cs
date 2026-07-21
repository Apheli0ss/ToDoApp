using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameIsCompletedToStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "TodoItems",
                newName: "Status");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "TodoItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.Sql("UPDATE [TodoItems] SET [Status] = 2 WHERE [Status] = 1;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [TodoItems] SET [Status] = 1 WHERE [Status] = 2;");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "TodoItems",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "TodoItems",
                newName: "IsCompleted");
        }
    }
}
