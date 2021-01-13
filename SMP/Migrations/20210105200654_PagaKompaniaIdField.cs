using Microsoft.EntityFrameworkCore.Migrations;

namespace SMP.Migrations
{
    public partial class PagaKompaniaIdField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KompaniaId",
                table: "PAGA",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PAGA_KompaniaId",
                table: "PAGA",
                column: "KompaniaId");

            migrationBuilder.AddForeignKey(
                name: "FK_PAGA_KOMPANIA_KompaniaId",
                table: "PAGA",
                column: "KompaniaId",
                principalTable: "KOMPANIA",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PAGA_KOMPANIA_KompaniaId",
                table: "PAGA");

            migrationBuilder.DropIndex(
                name: "IX_PAGA_KompaniaId",
                table: "PAGA");

            migrationBuilder.DropColumn(
                name: "KompaniaId",
                table: "PAGA");
        }
    }
}
