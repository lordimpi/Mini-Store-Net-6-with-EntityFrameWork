using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniStoreWeb.Data.Migrations
{
    public partial class AddBookImagesFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyProperty_Libros_LibroCodigo",
                table: "MyProperty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyProperty",
                table: "MyProperty");

            migrationBuilder.RenameTable(
                name: "MyProperty",
                newName: "BookImages");

            migrationBuilder.RenameIndex(
                name: "IX_MyProperty_LibroCodigo",
                table: "BookImages",
                newName: "IX_BookImages_LibroCodigo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookImages",
                table: "BookImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookImages_Libros_LibroCodigo",
                table: "BookImages",
                column: "LibroCodigo",
                principalTable: "Libros",
                principalColumn: "Codigo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookImages_Libros_LibroCodigo",
                table: "BookImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookImages",
                table: "BookImages");

            migrationBuilder.RenameTable(
                name: "BookImages",
                newName: "MyProperty");

            migrationBuilder.RenameIndex(
                name: "IX_BookImages_LibroCodigo",
                table: "MyProperty",
                newName: "IX_MyProperty_LibroCodigo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyProperty",
                table: "MyProperty",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MyProperty_Libros_LibroCodigo",
                table: "MyProperty",
                column: "LibroCodigo",
                principalTable: "Libros",
                principalColumn: "Codigo");
        }
    }
}
