using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMP.Migrations
{
    public partial class addTableKontrataPunetori : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PUNETORI_KONTRATA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PunetoriId = table.Column<int>(nullable: false),
                    Emri = table.Column<string>(fixedLength: true, maxLength: 10, nullable: false),
                    Path = table.Column<string>(maxLength: 200, nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PUNETORI_KONTRATA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PUNETORI_KONTRATA_PUNETORI",
                        column: x => x.PunetoriId,
                        principalTable: "PUNETORI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PUNETORI_KONTRATA_PunetoriId",
                table: "PUNETORI_KONTRATA",
                column: "PunetoriId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PUNETORI_KONTRATA");
        }
    }
}
