using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMP.Migrations
{
    public partial class PunetoriKontrataDroppd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PUNETORI_KONTRATA_PUNETORI",
                table: "PUNETORI_KONTRATA");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PUNETORI_KONTRATA",
                table: "PUNETORI_KONTRATA");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PunetoriKontrata_PUNETORI",
                table: "PunetoriKontrata");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PunetoriKontrata",
                table: "PunetoriKontrata");
        }
    }
}
