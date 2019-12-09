using Microsoft.EntityFrameworkCore.Migrations;

namespace SistemasWeb.Data.Migrations
{
    public partial class Migration9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoriaID",
                table: "_TInscripcion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaID",
                table: "_TInscripcion",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
