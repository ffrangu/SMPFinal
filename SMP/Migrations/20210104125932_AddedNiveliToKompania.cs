using Microsoft.EntityFrameworkCore.Migrations;

namespace SMP.Migrations
{
    public partial class AddedNiveliToKompania : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Niveli",
                table: "KOMPANIA",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Niveli",
                table: "KOMPANIA");
        }
    }
}
