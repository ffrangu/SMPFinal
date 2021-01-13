using Microsoft.EntityFrameworkCore.Migrations;

namespace SMP.Migrations
{
    public partial class BonusetNetoField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BonuseNeto",
                table: "PAGA",
                type: "decimal(18, 2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BonuseNeto",
                table: "PAGA");
        }
    }
}
