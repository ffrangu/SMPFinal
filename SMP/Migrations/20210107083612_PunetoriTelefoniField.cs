using Microsoft.EntityFrameworkCore.Migrations;

namespace SMP.Migrations
{
    public partial class PunetoriTelefoniField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Telefoni",
                table: "PUNETORI",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefoni",
                table: "PUNETORI");
        }
    }
}
