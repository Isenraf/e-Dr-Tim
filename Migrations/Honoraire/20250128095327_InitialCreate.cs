using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BANA.Migrations.Honoraire
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Honoraire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Numerofacture = table.Column<string>(type: "TEXT", nullable: true),
                    Medecin = table.Column<string>(type: "TEXT", nullable: true),
                    Telephone = table.Column<string>(type: "TEXT", nullable: true),
                    Patient = table.Column<string>(type: "TEXT", nullable: true),
                    Mode_paiement = table.Column<string>(type: "TEXT", nullable: true),
                    Assurance = table.Column<string>(type: "TEXT", nullable: true),
                    Etat = table.Column<string>(type: "TEXT", nullable: true),
                    Observation = table.Column<string>(type: "TEXT", nullable: true),
                    Create_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Paid_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Montant = table.Column<int>(type: "INTEGER", nullable: true),
                    Pourcentage = table.Column<decimal>(type: "TEXT", nullable: true),
                    Montant_medecin = table.Column<int>(type: "INTEGER", nullable: true),
                    Montant_paye = table.Column<int>(type: "INTEGER", nullable: true),
                    Montant_reste = table.Column<int>(type: "INTEGER", nullable: true),
                    Autocalcul = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Honoraire", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Honoraire");
        }
    }
}
