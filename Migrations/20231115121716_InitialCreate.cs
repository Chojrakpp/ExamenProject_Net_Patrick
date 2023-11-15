using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamenProject_Patrick.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Onderwerpen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Onderwerpen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Afspraken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OnderwerpId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Afspraken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Afspraken_Onderwerpen_OnderwerpId",
                        column: x => x.OnderwerpId,
                        principalTable: "Onderwerpen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Afspraken_OnderwerpId",
                table: "Afspraken",
                column: "OnderwerpId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Afspraken");

            migrationBuilder.DropTable(
                name: "Onderwerpen");
        }
    }
}
