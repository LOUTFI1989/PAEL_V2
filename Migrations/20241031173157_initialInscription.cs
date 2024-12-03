using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAEL_V2.Migrations
{
    /// <inheritdoc />
    public partial class initialInscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EtudiantNom",
                table: "Inscription",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FormationNom",
                table: "Inscription",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EtudiantNom",
                table: "Inscription");

            migrationBuilder.DropColumn(
                name: "FormationNom",
                table: "Inscription");
        }
    }
}
