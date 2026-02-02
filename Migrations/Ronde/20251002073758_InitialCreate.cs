using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BANA.Migrations.Ronde
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ronde",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroDossier = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroHospi = table.Column<string>(type: "TEXT", nullable: true),
                    Patient = table.Column<string>(type: "TEXT", nullable: true),
                    Pc = table.Column<string>(type: "TEXT", nullable: true),
                    Plainte = table.Column<string>(type: "TEXT", nullable: true),
                    Parametres = table.Column<string>(type: "TEXT", nullable: true),
                    Epj = table.Column<string>(type: "TEXT", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DerniereModif = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Trumps = table.Column<string>(type: "TEXT", nullable: false),
                    Trumps2 = table.Column<string>(type: "TEXT", nullable: false),
                    Trumps3 = table.Column<string>(type: "TEXT", nullable: false),
                    Trumps4 = table.Column<string>(type: "TEXT", nullable: false),
                    Medecin = table.Column<string>(type: "TEXT", nullable: true),
                    Etat = table.Column<string>(type: "TEXT", nullable: true),
                    Taf = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ronde", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ronde");
        }
    }
}
