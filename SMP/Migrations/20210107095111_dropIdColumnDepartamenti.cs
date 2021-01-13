using Microsoft.EntityFrameworkCore.Migrations;

namespace SMP.Migrations
{
    public partial class dropIdColumnDepartamenti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "DEPARTAMENTI");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DEPARTAMENTI",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
