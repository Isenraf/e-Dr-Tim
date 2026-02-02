using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BANA.Migrations.Chambre
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chambre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Numero_chambre = table.Column<string>(type: "TEXT", nullable: false),
                    Patient = table.Column<string>(type: "TEXT", nullable: true),
                    Create_date = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDebut = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateFin = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TypeChambre = table.Column<string>(type: "TEXT", nullable: false),
                    Prix = table.Column<int>(type: "INTEGER", nullable: true),
                    Busy = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chambre", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chambre");
        }
    }
}
