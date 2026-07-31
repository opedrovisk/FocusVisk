using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FocusVisk.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalendarNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PomodoroSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    WasCompleted = table.Column<bool>(type: "bit", nullable: false),
                    TaskTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PomodoroSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuickNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPinned = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuickNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PomodoroDurationMinutes = table.Column<int>(type: "int", nullable: false),
                    ShortBreakMinutes = table.Column<int>(type: "int", nullable: false),
                    LongBreakMinutes = table.Column<int>(type: "int", nullable: false),
                    SessionsBeforeLongBreak = table.Column<int>(type: "int", nullable: false),
                    PlaySounds = table.Column<bool>(type: "bit", nullable: false),
                    ShowNotifications = table.Column<bool>(type: "bit", nullable: false),
                    FocusBlockEnabled = table.Column<bool>(type: "bit", nullable: false),
                    BlockedSites = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Theme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccentColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SidebarBgColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainBgColor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "AccentColor", "BlockedSites", "FocusBlockEnabled", "LongBreakMinutes", "MainBgColor", "PlaySounds", "PomodoroDurationMinutes", "SessionsBeforeLongBreak", "ShortBreakMinutes", "ShowNotifications", "SidebarBgColor", "Theme" },
                values: new object[] { 1, "#7C6AF7", "youtube.com\nx.com\ninstagram.com\nreddit.com", false, 15, "#0F0F14", true, 25, 4, 5, true, "#16161E", "dark" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarNotes");

            migrationBuilder.DropTable(
                name: "PomodoroSessions");

            migrationBuilder.DropTable(
                name: "QuickNotes");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Todos");
        }
    }
}
