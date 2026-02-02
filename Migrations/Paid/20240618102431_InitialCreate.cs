using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BANA.Migrations.Paid
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Paid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Numero_dossier = table.Column<string>(type: "TEXT", nullable: true),
                    Patient = table.Column<string>(type: "TEXT", nullable: true),
                    Numero_facture = table.Column<string>(type: "TEXT", nullable: true),
                    Moyen_paiement = table.Column<string>(type: "TEXT", nullable: true),
                    Create_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Montant = table.Column<int>(type: "INTEGER", nullable: false),
                    Frais_retrait = table.Column<int>(type: "INTEGER", nullable: false),
                    Numerocaution = table.Column<string>(type: "TEXT", nullable: true),
                    Caissier = table.Column<string>(type: "TEXT", nullable: true),
                    caisse = table.Column<string>(type: "TEXT", nullable: true),
                    fact_type = table.Column<string>(type: "TEXT", nullable: true),
                    Observation = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paid", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Paid");
        }
    }
}
