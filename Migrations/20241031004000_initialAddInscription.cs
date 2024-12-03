using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PAEL_V2.Migrations
{
    /// <inheritdoc />
    public partial class initialAddInscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inscription",
                columns: table => new
                {
                    InscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormationId = table.Column<int>(type: "int", nullable: false),
                    EtudiantId = table.Column<int>(type: "int", nullable: false),
                    DateInscription = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CoursId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscription", x => x.InscriptionId);
                    table.ForeignKey(
                        name: "FK_Inscription_Cours_CoursId",
                        column: x => x.CoursId,
                        principalTable: "Cours",
                        principalColumn: "CoursId");
                    table.ForeignKey(
                        name: "FK_Inscription_Etudiant_EtudiantId",
                        column: x => x.EtudiantId,
                        principalTable: "Etudiant",
                        principalColumn: "EtudiantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscription_Formation_FormationId",
                        column: x => x.FormationId,
                        principalTable: "Formation",
                        principalColumn: "FormationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inscription_CoursId",
                table: "Inscription",
                column: "CoursId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscription_EtudiantId",
                table: "Inscription",
                column: "EtudiantId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscription_FormationId",
                table: "Inscription",
                column: "FormationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inscription");
        }
    }
}
