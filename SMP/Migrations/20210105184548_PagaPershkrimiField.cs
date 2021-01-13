using Microsoft.EntityFrameworkCore.Migrations;

namespace SMP.Migrations
{
    public partial class PagaPershkrimiField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pershkrimi",
                table: "PAGA",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pershkrimi",
                table: "PAGA");
        }
    }
}
