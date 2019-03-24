using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManager.DAL.Migrations
{
    public partial class TaskHistoryUp4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskChanges_Tasks_TaskItem",
                table: "TaskChanges");

            migrationBuilder.RenameColumn(
                name: "TaskItem",
                table: "TaskChanges",
                newName: "TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskChanges_TaskItem",
                table: "TaskChanges",
                newName: "IX_TaskChanges_TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskChanges_Tasks_TaskId",
                table: "TaskChanges",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskChanges_Tasks_TaskId",
                table: "TaskChanges");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "TaskChanges",
                newName: "TaskItem");

            migrationBuilder.RenameIndex(
                name: "IX_TaskChanges_TaskId",
                table: "TaskChanges",
                newName: "IX_TaskChanges_TaskItem");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskChanges_Tasks_TaskItem",
                table: "TaskChanges",
                column: "TaskItem",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
