using Microsoft.EntityFrameworkCore.Migrations;

namespace SMP.Migrations
{
    public partial class PunetoriEmailField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "PUNETORI",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "PUNETORI");
        }
    }
}
