using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniStoreWeb.Data.Migrations
{
    public partial class AddBookImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Libros");

            migrationBuilder.CreateTable(
                name: "MyProperty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LibroCodigo = table.Column<int>(type: "int", nullable: true),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyProperty_Libros_LibroCodigo",
                        column: x => x.LibroCodigo,
                        principalTable: "Libros",
                        principalColumn: "Codigo");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyProperty_LibroCodigo",
                table: "MyProperty",
                column: "LibroCodigo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyProperty");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Libros",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
