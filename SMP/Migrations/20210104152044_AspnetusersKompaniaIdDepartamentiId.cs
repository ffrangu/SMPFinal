using Microsoft.EntityFrameworkCore.Migrations;

namespace SMP.Migrations
{
    public partial class AspnetusersKompaniaIdDepartamentiId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartamentiId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KompaniaId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartamentiId",
                table: "AspNetUsers",
                column: "DepartamentiId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_KompaniaId",
                table: "AspNetUsers",
                column: "KompaniaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_DEPARTAMENTI_DepartamentiId",
                table: "AspNetUsers",
                column: "DepartamentiId",
                principalTable: "DEPARTAMENTI",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_KOMPANIA_KompaniaId",
                table: "AspNetUsers",
                column: "KompaniaId",
                principalTable: "KOMPANIA",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_DEPARTAMENTI_DepartamentiId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_KOMPANIA_KompaniaId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DepartamentiId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_KompaniaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DepartamentiId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "KompaniaId",
                table: "AspNetUsers");
        }
    }
}
