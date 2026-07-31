using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FocusVisk.Migrations
{
    /// <inheritdoc />
    public partial class AddTagToQuickNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "QuickNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStreakAlertAt",
                table: "Habits",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tag",
                table: "QuickNotes");

            migrationBuilder.DropColumn(
                name: "LastStreakAlertAt",
                table: "Habits");
        }
    }
}
