using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniStoreWeb.Data.Migrations
{
    public partial class AddStockColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Stock",
                table: "Libros",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Libros");
        }
    }
}
