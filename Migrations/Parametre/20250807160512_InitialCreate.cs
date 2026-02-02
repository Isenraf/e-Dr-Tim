using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BANA.Migrations.Parametre
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parametre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Numero_Dossier = table.Column<string>(type: "TEXT", nullable: true),
                    Numero_Facture = table.Column<string>(type: "TEXT", nullable: true),
                    Patient = table.Column<string>(type: "TEXT", nullable: true),
                    Temperature = table.Column<string>(type: "TEXT", nullable: true),
                    Poids = table.Column<string>(type: "TEXT", nullable: true),
                    Taille = table.Column<string>(type: "TEXT", nullable: true),
                    Saturation = table.Column<string>(type: "TEXT", nullable: true),
                    Rpm = table.Column<string>(type: "TEXT", nullable: true),
                    Tsysbg = table.Column<string>(type: "TEXT", nullable: true),
                    Tsysbd = table.Column<string>(type: "TEXT", nullable: true),
                    Tdiasbg = table.Column<string>(type: "TEXT", nullable: true),
                    Tdiasbd = table.Column<string>(type: "TEXT", nullable: true),
                    Pbg = table.Column<string>(type: "TEXT", nullable: true),
                    Pbd = table.Column<string>(type: "TEXT", nullable: true),
                    Glycemie_capillaire = table.Column<string>(type: "TEXT", nullable: true),
                    Pc = table.Column<string>(type: "TEXT", nullable: true),
                    Infirmier = table.Column<string>(type: "TEXT", nullable: true),
                    Pt = table.Column<string>(type: "TEXT", nullable: true),
                    Pb = table.Column<string>(type: "TEXT", nullable: true),
                    Create_date = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Observation = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parametre", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parametre");
        }
    }
}
