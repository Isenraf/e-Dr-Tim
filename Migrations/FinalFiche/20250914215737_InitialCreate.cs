using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BANA.Migrations.FinalFiche
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinalFiche",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: true),
                    Patient = table.Column<string>(type: "TEXT", nullable: true),
                    Numero_dossier = table.Column<string>(type: "TEXT", nullable: true),
                    DatedeNaissance = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Genre = table.Column<string>(type: "TEXT", nullable: true),
                    Contenu = table.Column<string>(type: "TEXT", nullable: true),
                    Categorie = table.Column<string>(type: "TEXT", nullable: true),
                    Create_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Dernieremodification = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Trump1 = table.Column<string>(type: "TEXT", nullable: true),
                    Trump2 = table.Column<string>(type: "TEXT", nullable: true),
                    Trump3 = table.Column<string>(type: "TEXT", nullable: true),
                    Etat = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalFiche", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinalFiche");
        }
    }
}
