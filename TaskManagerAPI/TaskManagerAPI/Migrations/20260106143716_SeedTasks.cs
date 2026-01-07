using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CreatedAt", "Description", "DueDate", "IsCompleted", "Priority", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 6, 16, 37, 15, 553, DateTimeKind.Local).AddTicks(8085), "Initialize TaskManagerAPI", new DateTime(2026, 1, 7, 16, 37, 15, 553, DateTimeKind.Local).AddTicks(8063), false, 1, "Setup project" },
                    { 2, new DateTime(2026, 1, 6, 16, 37, 15, 553, DateTimeKind.Local).AddTicks(8089), "Create EF Core models", new DateTime(2026, 1, 8, 16, 37, 15, 553, DateTimeKind.Local).AddTicks(8087), false, 2, "Design database" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
