using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FocusVisk.Migrations
{
    public partial class AddSubTasksAndCalendar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Mostrar tarefa no calendário
            migrationBuilder.AddColumn<bool>(
                name: "ShowInCalendar",
                table: "Todos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // FK para tarefa pai (self-referencing)
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Todos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Todos_ParentId",
                table: "Todos",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Todos_ParentId",
                table: "Todos",
                column: "ParentId",
                principalTable: "Todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Todos_ParentId",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_ParentId",
                table: "Todos");

            migrationBuilder.DropColumn(name: "ParentId", table: "Todos");
            migrationBuilder.DropColumn(name: "ShowInCalendar", table: "Todos");
        }
    }
}