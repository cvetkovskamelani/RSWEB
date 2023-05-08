using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Migrations
{
    public partial class DatabaseRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBookss_Books_BookId",
                table: "UserBookss");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBookss",
                table: "UserBookss");

            migrationBuilder.RenameTable(
                name: "UserBookss",
                newName: "UserBooks");

            migrationBuilder.RenameIndex(
                name: "IX_UserBookss_BookId",
                table: "UserBooks",
                newName: "IX_UserBooks_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBooks",
                table: "UserBooks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBooks_Books_BookId",
                table: "UserBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBooks_Books_BookId",
                table: "UserBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBooks",
                table: "UserBooks");

            migrationBuilder.RenameTable(
                name: "UserBooks",
                newName: "UserBookss");

            migrationBuilder.RenameIndex(
                name: "IX_UserBooks_BookId",
                table: "UserBookss",
                newName: "IX_UserBookss_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBookss",
                table: "UserBookss",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBookss_Books_BookId",
                table: "UserBookss",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
