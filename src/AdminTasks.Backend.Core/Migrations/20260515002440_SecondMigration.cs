using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminTasks.Backend.Core.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tareas",
                table: "Tareas");

            migrationBuilder.RenameTable(
                name: "Tareas",
                newName: "tareas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tareas",
                table: "tareas",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tareas",
                table: "tareas");

            migrationBuilder.RenameTable(
                name: "tareas",
                newName: "Tareas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tareas",
                table: "Tareas",
                column: "Id");
        }
    }
}
