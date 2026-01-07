using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTagsColumnToTaskItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Tasks",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DueDate", "Tags" },
                values: new object[] { new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Work" });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DueDate", "Tags" },
                values: new object[] { new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Work" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CreatedAt", "Description", "DueDate", "IsCompleted", "Priority", "Tags", "Title" },
                values: new object[] { 3, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finish reading personal development book", new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 3, "Personal", "Read book" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Tasks");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "DueDate" },
                values: new object[] { new DateTime(2026, 1, 6, 16, 37, 15, 553, DateTimeKind.Local).AddTicks(8085), new DateTime(2026, 1, 7, 16, 37, 15, 553, DateTimeKind.Local).AddTicks(8063) });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DueDate" },
                values: new object[] { new DateTime(2026, 1, 6, 16, 37, 15, 553, DateTimeKind.Local).AddTicks(8089), new DateTime(2026, 1, 8, 16, 37, 15, 553, DateTimeKind.Local).AddTicks(8087) });
        }
    }
}
