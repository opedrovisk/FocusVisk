using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FocusVisk.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskAutomation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRecurring",
                table: "Todos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAlertFiredAt",
                table: "Todos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecurrenceType",
                table: "Todos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ScheduledTime",
                table: "Todos",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRecurring",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "LastAlertFiredAt",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "RecurrenceType",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "ScheduledTime",
                table: "Todos");
        }
    }
}
