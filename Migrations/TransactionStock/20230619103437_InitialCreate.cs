using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BANA.Migrations.TransactionStock
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom_article = table.Column<string>(type: "TEXT", nullable: false),
                    Emplacement = table.Column<string>(type: "TEXT", nullable: false),
                    Quantitee = table.Column<decimal>(type: "TEXT", nullable: false),
                    utilisateur = table.Column<string>(type: "TEXT", nullable: true),
                    Observation = table.Column<string>(type: "TEXT", nullable: true),
                    TypeOperation = table.Column<string>(type: "TEXT", nullable: true),
                    Create_date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionStock", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionStock");
        }
    }
}
