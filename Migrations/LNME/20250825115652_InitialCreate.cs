using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BANA.Migrations.LNME
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LNME",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: true),
                    DCI = table.Column<string>(type: "TEXT", nullable: true),
                    Catégorie = table.Column<string>(type: "TEXT", nullable: true),
                    Dosage = table.Column<string>(type: "TEXT", nullable: true),
                    Forme = table.Column<string>(type: "TEXT", nullable: true),
                    actif = table.Column<bool>(type: "INTEGER", nullable: false),
                    Create_date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LNME", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LNME");
        }
    }
}
