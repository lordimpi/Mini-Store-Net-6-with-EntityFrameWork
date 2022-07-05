using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniStoreWeb.Migrations
{
    public partial class AddTemporalSaleEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TemporalSales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LibroCodigo = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<float>(type: "real", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporalSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemporalSales_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TemporalSales_Libros_LibroCodigo",
                        column: x => x.LibroCodigo,
                        principalTable: "Libros",
                        principalColumn: "Codigo");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemporalSales_LibroCodigo",
                table: "TemporalSales",
                column: "LibroCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_TemporalSales_UserId",
                table: "TemporalSales",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemporalSales");
        }
    }
}
