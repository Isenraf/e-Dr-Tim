using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BANA.Migrations.Hospi
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hospi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: true),
                    Numero_dossier = table.Column<string>(type: "TEXT", nullable: true),
                    Sexe = table.Column<string>(type: "TEXT", nullable: true),
                    DateNaissance = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Adresse = table.Column<string>(type: "TEXT", nullable: true),
                    Telephone = table.Column<string>(type: "TEXT", nullable: true),
                    PersonneContactUrgence = table.Column<string>(type: "TEXT", nullable: true),
                    Numero_facture = table.Column<string>(type: "TEXT", nullable: true),
                    TelephoneContactUrgence = table.Column<string>(type: "TEXT", nullable: true),
                    GroupeSanguin = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroHospitalisation = table.Column<string>(type: "TEXT", nullable: true),
                    DateAdmission = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MotifAdmission = table.Column<string>(type: "TEXT", nullable: true),
                    TypeHospitalisation = table.Column<string>(type: "TEXT", nullable: true),
                    Service = table.Column<string>(type: "TEXT", nullable: true),
                    Chambre = table.Column<string>(type: "TEXT", nullable: true),
                    Lit = table.Column<string>(type: "TEXT", nullable: true),
                    MedecinResponsable = table.Column<string>(type: "TEXT", nullable: true),
                    DiagnosticAdmission = table.Column<string>(type: "TEXT", nullable: true),
                    Temperature = table.Column<decimal>(type: "TEXT", nullable: false),
                    TensionArterielle = table.Column<string>(type: "TEXT", nullable: true),
                    Pouls = table.Column<int>(type: "INTEGER", nullable: false),
                    FrequenceRespiratoire = table.Column<int>(type: "INTEGER", nullable: false),
                    SaturationOxygene = table.Column<int>(type: "INTEGER", nullable: false),
                    Allergies = table.Column<string>(type: "TEXT", nullable: true),
                    Antecedents = table.Column<string>(type: "TEXT", nullable: true),
                    TraitementsEnCours = table.Column<string>(type: "TEXT", nullable: true),
                    Prescriptions = table.Column<string>(type: "TEXT", nullable: true),
                    ActesMedicaux = table.Column<string>(type: "TEXT", nullable: true),
                    ExamensEtResultats = table.Column<string>(type: "TEXT", nullable: true),
                    NotesInfirmieres = table.Column<string>(type: "TEXT", nullable: true),
                    EvolutionClinique = table.Column<string>(type: "TEXT", nullable: true),
                    DateSortie = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DiagnosticSortie = table.Column<string>(type: "TEXT", nullable: true),
                    EtatSortie = table.Column<string>(type: "TEXT", nullable: true),
                    Destination = table.Column<string>(type: "TEXT", nullable: true),
                    ResumeMedical = table.Column<string>(type: "TEXT", nullable: true),
                    OrdonnanceSortie = table.Column<string>(type: "TEXT", nullable: true),
                    Assurance = table.Column<string>(type: "TEXT", nullable: true),
                    CoutSejour = table.Column<decimal>(type: "TEXT", nullable: false),
                    PaiementEffectue = table.Column<decimal>(type: "TEXT", nullable: false),
                    SoldeRestant = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospi", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hospi");
        }
    }
}
