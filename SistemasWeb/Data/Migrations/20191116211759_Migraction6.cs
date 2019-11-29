using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SistemasWeb.Data.Migrations
{
    public partial class Migraction6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_TCategoria",
                columns: table => new
                {
                    CategoriaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: false),
                    Descripcion = table.Column<string>(nullable: false),
                    Estado = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TCategoria", x => x.CategoriaID);
                });

            migrationBuilder.CreateTable(
                name: "_TCursos",
                columns: table => new
                {
                    CursoID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true),
                    Horas = table.Column<byte>(nullable: false),
                    Costo = table.Column<decimal>(nullable: false),
                    Estado = table.Column<bool>(nullable: false),
                    CategoriaID = table.Column<int>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TCursos", x => x.CursoID);
                    table.ForeignKey(
                        name: "FK__TCursos__TCategoria_CategoriaID",
                        column: x => x.CategoriaID,
                        principalTable: "_TCategoria",
                        principalColumn: "CategoriaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX__TCursos_CategoriaID",
                table: "_TCursos",
                column: "CategoriaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_TCursos");

            migrationBuilder.DropTable(
                name: "_TCategoria");
        }
    }
}
