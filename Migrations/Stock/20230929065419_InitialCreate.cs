using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BANA.Migrations.Stock
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom_article = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Prix_vente = table.Column<decimal>(type: "TEXT", nullable: false),
                    Prix_achat = table.Column<decimal>(type: "TEXT", nullable: true),
                    Quantitee = table.Column<decimal>(type: "TEXT", nullable: true),
                    In = table.Column<int>(type: "INTEGER", nullable: true),
                    Out = table.Column<int>(type: "INTEGER", nullable: true),
                    Vendu = table.Column<int>(type: "INTEGER", nullable: true),
                    Stock_minimal = table.Column<decimal>(type: "TEXT", nullable: true),
                    chiffre_affaire = table.Column<decimal>(type: "TEXT", nullable: true),
                    Categorie = table.Column<string>(type: "TEXT", nullable: true),
                    Reference = table.Column<string>(type: "TEXT", nullable: true),
                    codebarre = table.Column<string>(type: "TEXT", nullable: true),
                    Lieu_stockage = table.Column<string>(type: "TEXT", nullable: true),
                    unité = table.Column<string>(type: "TEXT", nullable: true),
                    Actif = table.Column<bool>(type: "INTEGER", nullable: false),
                    Compte_general = table.Column<string>(type: "TEXT", nullable: true),
                    Img = table.Column<string>(type: "TEXT", nullable: true),
                    CreateBy = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    Societe = table.Column<string>(type: "TEXT", nullable: true),
                    Compte_general_entree = table.Column<string>(type: "TEXT", nullable: true),
                    Compte_general_sortie = table.Column<string>(type: "TEXT", nullable: true),
                    Create_date = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModification = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stock");
        }
    }
}
