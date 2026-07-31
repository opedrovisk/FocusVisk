using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FocusVisk.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ===== Tabelas novas do Identity (não existiam no banco) =====
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            // ===== Tabelas que JÁ EXISTEM (do Desktop) — só ganham a coluna UserId =====
            // Nullable por enquanto, pra não quebrar as linhas que já existem
            migrationBuilder.AddColumn<string>(name: "UserId", table: "CalendarNotes", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "UserId", table: "Habits", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "UserId", table: "PomodoroSessions", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "UserId", table: "QuickNotes", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "UserId", table: "SavingGoals", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "UserId", table: "Settings", type: "nvarchar(450)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "UserId", table: "Todos", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "UserId", table: "Transactions", type: "nvarchar(max)", nullable: true);

            // ===== Seed do usuário "Desktop" + associação dos dados que já existiam =====
            var desktopUserId = "00000000-0000-0000-0000-000000000001";

            migrationBuilder.Sql($@"
                IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE Id = '{desktopUserId}')
                INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
                    PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled,
                    LockoutEnabled, AccessFailedCount)
                VALUES ('{desktopUserId}', 'desktop@local', 'DESKTOP@LOCAL', 'desktop@local', 'DESKTOP@LOCAL', 1,
                    '', NEWID(), NEWID(), 0, 0, 0, 0);
            ");

            migrationBuilder.Sql($"UPDATE CalendarNotes SET UserId = '{desktopUserId}' WHERE UserId IS NULL;");
            migrationBuilder.Sql($"UPDATE Habits SET UserId = '{desktopUserId}' WHERE UserId IS NULL;");
            migrationBuilder.Sql($"UPDATE PomodoroSessions SET UserId = '{desktopUserId}' WHERE UserId IS NULL;");
            migrationBuilder.Sql($"UPDATE QuickNotes SET UserId = '{desktopUserId}' WHERE UserId IS NULL;");
            migrationBuilder.Sql($"UPDATE SavingGoals SET UserId = '{desktopUserId}' WHERE UserId IS NULL;");
            migrationBuilder.Sql($"UPDATE Settings SET UserId = '{desktopUserId}' WHERE UserId IS NULL;");
            migrationBuilder.Sql($"UPDATE Todos SET UserId = '{desktopUserId}' WHERE UserId IS NULL;");
            migrationBuilder.Sql($"UPDATE Transactions SET UserId = '{desktopUserId}' WHERE UserId IS NULL;");

            // ===== Agora sim, torna UserId obrigatório =====
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "CalendarNotes", type: "nvarchar(max)", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(max)", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "Habits", type: "nvarchar(max)", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(max)", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "PomodoroSessions", type: "nvarchar(max)", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(max)", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "QuickNotes", type: "nvarchar(max)", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(max)", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "SavingGoals", type: "nvarchar(max)", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(max)", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "Settings", type: "nvarchar(450)", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(450)", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "Todos", type: "nvarchar(max)", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(max)", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "Transactions", type: "nvarchar(max)", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(max)", oldNullable: true);

            // Índice novo: 1 Settings por usuário (não existia antes, Settings era registro único global)
            migrationBuilder.CreateIndex(
                name: "IX_Settings_UserId",
                table: "Settings",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Settings_UserId", table: "Settings");

            migrationBuilder.DropColumn(name: "UserId", table: "CalendarNotes");
            migrationBuilder.DropColumn(name: "UserId", table: "Habits");
            migrationBuilder.DropColumn(name: "UserId", table: "PomodoroSessions");
            migrationBuilder.DropColumn(name: "UserId", table: "QuickNotes");
            migrationBuilder.DropColumn(name: "UserId", table: "SavingGoals");
            migrationBuilder.DropColumn(name: "UserId", table: "Settings");
            migrationBuilder.DropColumn(name: "UserId", table: "Todos");
            migrationBuilder.DropColumn(name: "UserId", table: "Transactions");

            migrationBuilder.Sql("DELETE FROM AspNetUsers WHERE Id = '00000000-0000-0000-0000-000000000001';");

            migrationBuilder.DropTable(name: "AspNetRoleClaims");
            migrationBuilder.DropTable(name: "AspNetUserClaims");
            migrationBuilder.DropTable(name: "AspNetUserLogins");
            migrationBuilder.DropTable(name: "AspNetUserRoles");
            migrationBuilder.DropTable(name: "AspNetUserTokens");
            migrationBuilder.DropTable(name: "AspNetRoles");
            migrationBuilder.DropTable(name: "AspNetUsers");
        }
    }
}