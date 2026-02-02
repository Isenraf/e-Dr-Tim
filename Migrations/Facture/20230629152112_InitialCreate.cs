using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BANA.Migrations.Facture
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Prenom = table.Column<string>(type: "TEXT", nullable: true),
                    Numero_dossier = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Taille = table.Column<string>(type: "TEXT", nullable: true),
                    Medecin = table.Column<string>(type: "TEXT", nullable: true),
                    Create_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Date_validite = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Derniere_modification = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Derniere_impression = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateNaissance = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Genre = table.Column<string>(type: "TEXT", nullable: false),
                    Profession = table.Column<string>(type: "TEXT", nullable: true),
                    Quartier = table.Column<string>(type: "TEXT", nullable: true),
                    GroupeSanguin = table.Column<string>(type: "TEXT", nullable: true),
                    Intervenant = table.Column<string>(type: "TEXT", nullable: true),
                    Ligne_facturation1 = table.Column<string>(type: "TEXT", nullable: true),
                    Ligne_facturation2 = table.Column<string>(type: "TEXT", nullable: true),
                    Ligne_facturation3 = table.Column<string>(type: "TEXT", nullable: true),
                    Ligne_facturation4 = table.Column<string>(type: "TEXT", nullable: true),
                    Numero_de_facture = table.Column<string>(type: "TEXT", nullable: true),
                    Moyen_paiement = table.Column<string>(type: "TEXT", nullable: true),
                    Assur_Adresse = table.Column<string>(type: "TEXT", nullable: true),
                    Assur_BP = table.Column<string>(type: "TEXT", nullable: true),
                    Assur_Tel = table.Column<string>(type: "TEXT", nullable: true),
                    Assur_NIU = table.Column<string>(type: "TEXT", nullable: true),
                    Assur_Rc = table.Column<string>(type: "TEXT", nullable: true),
                    Total_ht = table.Column<int>(type: "INTEGER", nullable: false),
                    Tva = table.Column<int>(type: "INTEGER", nullable: false),
                    Remise = table.Column<int>(type: "INTEGER", nullable: false),
                    Taxe1 = table.Column<float>(type: "REAL", nullable: false),
                    Total_ttc = table.Column<int>(type: "INTEGER", nullable: false),
                    Net_a_payer_patient = table.Column<int>(type: "INTEGER", nullable: false),
                    Montant_recu_patient = table.Column<int>(type: "INTEGER", nullable: false),
                    Montant_recu_assurance = table.Column<int>(type: "INTEGER", nullable: false),
                    Assureur = table.Column<string>(type: "TEXT", nullable: true),
                    Pourcentage_Assurance = table.Column<int>(type: "INTEGER", nullable: false),
                    Net_a_payer_assurance = table.Column<int>(type: "INTEGER", nullable: false),
                    Especes = table.Column<int>(type: "INTEGER", nullable: false),
                    frais_retrait = table.Column<int>(type: "INTEGER", nullable: false),
                    Nombre_affichage = table.Column<int>(type: "INTEGER", nullable: false),
                    Nombre_impression = table.Column<int>(type: "INTEGER", nullable: false),
                    ticketmoderateur = table.Column<int>(type: "INTEGER", nullable: false),
                    Montant_en_lettre = table.Column<string>(type: "TEXT", nullable: true),
                    Montant_en_lettre_assurance = table.Column<string>(type: "TEXT", nullable: true),
                    Montant_en_lettre_patient = table.Column<string>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    Matricule_patient = table.Column<string>(type: "TEXT", nullable: true),
                    MatriculeADH = table.Column<string>(type: "TEXT", nullable: true),
                    Societe = table.Column<string>(type: "TEXT", nullable: true),
                    assure_prin = table.Column<string>(type: "TEXT", nullable: true),
                    Date_entree = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Date_sortie = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Titre1 = table.Column<string>(type: "TEXT", nullable: true),
                    Titre2 = table.Column<string>(type: "TEXT", nullable: true),
                    Titre3 = table.Column<string>(type: "TEXT", nullable: true),
                    Titre4 = table.Column<string>(type: "TEXT", nullable: true),
                    Etat_patient = table.Column<string>(type: "TEXT", nullable: true),
                    Etat_assurance = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Facture_par = table.Column<string>(type: "TEXT", nullable: true),
                    Encaisse_par = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facture", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facture");
        }
    }
}
