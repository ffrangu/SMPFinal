using Microsoft.EntityFrameworkCore.Migrations;

namespace SMP.Migrations
{
    public partial class AddDepartamentiIdIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_DEPARTAMENTI_DepartamentiId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_POZITA_DEPARTAMENTI",
                table: "POZITA");

            migrationBuilder.DropForeignKey(
                name: "FK_PUNETORI_DEPARTAMENTI",
                table: "PUNETORI");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DEPARTAMENTI",
                table: "DEPARTAMENTI");

            migrationBuilder.DropColumn(
                name: "testid",
                table: "DEPARTAMENTI");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DEPARTAMENTI",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DEPARTAMENTI",
                table: "DEPARTAMENTI",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_DEPARTAMENTI_DepartamentiId",
                table: "AspNetUsers",
                column: "DepartamentiId",
                principalTable: "DEPARTAMENTI",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POZITA_DEPARTAMENTI",
                table: "POZITA",
                column: "DepartamentiId",
                principalTable: "DEPARTAMENTI",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PUNETORI_DEPARTAMENTI",
                table: "PUNETORI",
                column: "DepartamentiId",
                principalTable: "DEPARTAMENTI",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_DEPARTAMENTI_DepartamentiId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_POZITA_DEPARTAMENTI",
                table: "POZITA");

            migrationBuilder.DropForeignKey(
                name: "FK_PUNETORI_DEPARTAMENTI",
                table: "PUNETORI");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DEPARTAMENTI",
                table: "DEPARTAMENTI");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DEPARTAMENTI");

            migrationBuilder.AddColumn<int>(
                name: "testid",
                table: "DEPARTAMENTI",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DEPARTAMENTI",
                table: "DEPARTAMENTI",
                column: "testid");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_DEPARTAMENTI_DepartamentiId",
                table: "AspNetUsers",
                column: "DepartamentiId",
                principalTable: "DEPARTAMENTI",
                principalColumn: "testid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_POZITA_DEPARTAMENTI",
                table: "POZITA",
                column: "DepartamentiId",
                principalTable: "DEPARTAMENTI",
                principalColumn: "testid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PUNETORI_DEPARTAMENTI",
                table: "PUNETORI",
                column: "DepartamentiId",
                principalTable: "DEPARTAMENTI",
                principalColumn: "testid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
