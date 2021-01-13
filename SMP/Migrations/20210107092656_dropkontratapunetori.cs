using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMP.Migrations
{
    public partial class dropkontratapunetori : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Punetori_Kontrata");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Punetori_Kontrata",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Created = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Emri = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PunetoriId = table.Column<int>(type: "int", nullable: false),
            //        Status = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PunetoriKontrata", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_PunetoriKontrata_PUNETORI_PunetoriId",
            //            column: x => x.PunetoriId,
            //            principalTable: "PUNETORI",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_PunetoriKontrata_PunetoriId",
            //    table: "PunetoriKontrata",
            //    column: "PunetoriId");
        }
    }
}
