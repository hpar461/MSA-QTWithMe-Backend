using Microsoft.EntityFrameworkCore.Migrations;

namespace QTWithMe.Migrations
{
    public partial class FixedQt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_QTs_QTId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "QTId",
                table: "Comments",
                newName: "QtId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_QTId",
                table: "Comments",
                newName: "IX_Comments_QtId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_QTs_QtId",
                table: "Comments",
                column: "QtId",
                principalTable: "QTs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_QTs_QtId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "QtId",
                table: "Comments",
                newName: "QTId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_QtId",
                table: "Comments",
                newName: "IX_Comments_QTId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_QTs_QTId",
                table: "Comments",
                column: "QTId",
                principalTable: "QTs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
