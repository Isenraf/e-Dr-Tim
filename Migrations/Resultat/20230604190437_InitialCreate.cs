using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BANA.Migrations.Resultat
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Resultat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroDossier = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroFacture = table.Column<string>(type: "TEXT", nullable: true),
                    NomPatient = table.Column<string>(type: "TEXT", nullable: true),
                    Genre = table.Column<string>(type: "TEXT", nullable: true),
                    Telephone_patient = table.Column<string>(type: "TEXT", nullable: true),
                    Age_patient = table.Column<string>(type: "TEXT", nullable: true),
                    Prescripteur = table.Column<string>(type: "TEXT", nullable: true),
                    Contenu = table.Column<string>(type: "TEXT", nullable: true),
                    Categorie = table.Column<string>(type: "TEXT", nullable: true),
                    Cote = table.Column<string>(type: "TEXT", nullable: true),
                    Code = table.Column<string>(type: "TEXT", nullable: true),
                    ExamType = table.Column<string>(type: "TEXT", nullable: true),
                    fait_par = table.Column<string>(type: "TEXT", nullable: true),
                    valide_par = table.Column<string>(type: "TEXT", nullable: true),
                    Create_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Date_de_finalisation = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Date_de_naissance = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Montant = table.Column<int>(type: "INTEGER", nullable: false),
                    resultat_final = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resultat", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Resultat");
        }
    }
}
