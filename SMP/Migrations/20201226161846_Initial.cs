using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMP.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BANKA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kodi = table.Column<string>(maxLength: 10, nullable: false),
                    Emri = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BANKA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GRADA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Emri = table.Column<string>(maxLength: 100, nullable: false),
                    PagaMujore = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PagaVjetore = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRADA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KOMUNA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Emri = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KOMUNA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LOG_USERACTIVITY",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(maxLength: 128, nullable: false),
                    Activity = table.Column<string>(maxLength: 500, nullable: false),
                    HttpMethod = table.Column<string>(maxLength: 500, nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOG_USERACTIVITY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TATIMI",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerqindjaZero = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    VleraPare = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PerqindjaPare = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    VleraDyte = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PerqindjaDyte = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    VleraTrete = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PerqindjaTrete = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TATIMI", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KOMPANIA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    KomunaId = table.Column<int>(nullable: false),
                    Kodi = table.Column<string>(maxLength: 10, nullable: false),
                    Emri = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KOMPANIA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KOMPANIA_KOMUNA",
                        column: x => x.KomunaId,
                        principalTable: "KOMUNA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DEPARTAMENTI",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    KompaniaId = table.Column<int>(nullable: false),
                    Emri = table.Column<string>(maxLength: 250, nullable: false),
                    Shkurtesa = table.Column<string>(maxLength: 10, nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEPARTAMENTI", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DEPARTAMENTI_KOMPANIA",
                        column: x => x.KompaniaId,
                        principalTable: "KOMPANIA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "POZITA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KompaniaId = table.Column<int>(nullable: false),
                    DepartamentiId = table.Column<int>(nullable: false),
                    Emri = table.Column<string>(maxLength: 100, nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POZITA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POZITA_DEPARTAMENTI",
                        column: x => x.DepartamentiId,
                        principalTable: "DEPARTAMENTI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_POZITA_KOMPANIA",
                        column: x => x.KompaniaId,
                        principalTable: "KOMPANIA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PUNETORI",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(maxLength: 128, nullable: true),
                    Emri = table.Column<string>(maxLength: 250, nullable: false),
                    Mbiemri = table.Column<string>(maxLength: 250, nullable: false),
                    NumriPersonal = table.Column<string>(maxLength: 10, nullable: false),
                    Datelindja = table.Column<DateTime>(type: "datetime", nullable: false),
                    Adresa = table.Column<string>(maxLength: 250, nullable: true),
                    KomunaId = table.Column<int>(nullable: false),
                    KompaniaId = table.Column<int>(nullable: false),
                    DepartamentiId = table.Column<int>(nullable: false),
                    PozitaId = table.Column<int>(nullable: false),
                    BankaId = table.Column<int>(nullable: false),
                    Xhirollogaria = table.Column<string>(maxLength: 16, nullable: false),
                    GradaId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PUNETORI", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PUNETORI_BANKA",
                        column: x => x.BankaId,
                        principalTable: "BANKA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PUNETORI_DEPARTAMENTI",
                        column: x => x.DepartamentiId,
                        principalTable: "DEPARTAMENTI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PUNETORI_GRADA",
                        column: x => x.GradaId,
                        principalTable: "GRADA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PUNETORI_KOMPANIA",
                        column: x => x.KompaniaId,
                        principalTable: "KOMPANIA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PUNETORI_KOMUNA",
                        column: x => x.KomunaId,
                        principalTable: "KOMUNA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PUNETORI_POZITA",
                        column: x => x.PozitaId,
                        principalTable: "POZITA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BONUSET",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Muaji = table.Column<int>(nullable: false),
                    Viti = table.Column<int>(nullable: false),
                    PunetoriId = table.Column<int>(nullable: false),
                    Pershkrimi = table.Column<string>(maxLength: 250, nullable: false),
                    Vlera = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Bruto = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BONUSET", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BONUSET_PUNETORI",
                        column: x => x.PunetoriId,
                        principalTable: "PUNETORI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PAGA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PunetoriId = table.Column<int>(nullable: false),
                    GradaId = table.Column<int>(nullable: false),
                    Viti = table.Column<int>(nullable: false),
                    Muaji = table.Column<int>(nullable: false),
                    Bruto = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    KontributiPunetori = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    KontributiPunedhenesi = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PagaTatim = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Tatimi = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PagaNeto = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Bonuse = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    PagaFinale = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    MenyraEkzekutimit = table.Column<int>(nullable: false),
                    DataEkzekutimit = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAGA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PAGA_GRADA",
                        column: x => x.GradaId,
                        principalTable: "GRADA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PAGA_PUNETORI",
                        column: x => x.PunetoriId,
                        principalTable: "PUNETORI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PUNETORI_KONTRATA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    PunetoriId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Emri = table.Column<string>(fixedLength: true, maxLength: 10, nullable: false),
                    Path = table.Column<string>(maxLength: 200, nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PUNETORI_KONTRATA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PUNETORI_KONTRATA_PUNETORI",
                        column: x => x.PunetoriId,
                        principalTable: "PUNETORI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BONUSET_PunetoriId",
                table: "BONUSET",
                column: "PunetoriId");

            migrationBuilder.CreateIndex(
                name: "IX_DEPARTAMENTI_KompaniaId",
                table: "DEPARTAMENTI",
                column: "KompaniaId");

            migrationBuilder.CreateIndex(
                name: "IX_KOMPANIA_KomunaId",
                table: "KOMPANIA",
                column: "KomunaId");

            migrationBuilder.CreateIndex(
                name: "IX_PAGA_GradaId",
                table: "PAGA",
                column: "GradaId");

            migrationBuilder.CreateIndex(
                name: "IX_PAGA_PunetoriId",
                table: "PAGA",
                column: "PunetoriId");

            migrationBuilder.CreateIndex(
                name: "IX_POZITA_DepartamentiId",
                table: "POZITA",
                column: "DepartamentiId");

            migrationBuilder.CreateIndex(
                name: "IX_POZITA_KompaniaId",
                table: "POZITA",
                column: "KompaniaId");

            migrationBuilder.CreateIndex(
                name: "IX_PUNETORI_BankaId",
                table: "PUNETORI",
                column: "BankaId");

            migrationBuilder.CreateIndex(
                name: "IX_PUNETORI_DepartamentiId",
                table: "PUNETORI",
                column: "DepartamentiId");

            migrationBuilder.CreateIndex(
                name: "IX_PUNETORI_GradaId",
                table: "PUNETORI",
                column: "GradaId");

            migrationBuilder.CreateIndex(
                name: "IX_PUNETORI_KompaniaId",
                table: "PUNETORI",
                column: "KompaniaId");

            migrationBuilder.CreateIndex(
                name: "IX_PUNETORI_KomunaId",
                table: "PUNETORI",
                column: "KomunaId");

            migrationBuilder.CreateIndex(
                name: "IX_PUNETORI_PozitaId",
                table: "PUNETORI",
                column: "PozitaId");

            migrationBuilder.CreateIndex(
                name: "IX_PUNETORI_KONTRATA_PunetoriId",
                table: "PUNETORI_KONTRATA",
                column: "PunetoriId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BONUSET");

            migrationBuilder.DropTable(
                name: "LOG_USERACTIVITY");

            migrationBuilder.DropTable(
                name: "PAGA");

            migrationBuilder.DropTable(
                name: "PUNETORI_KONTRATA");

            migrationBuilder.DropTable(
                name: "TATIMI");

            migrationBuilder.DropTable(
                name: "PUNETORI");

            migrationBuilder.DropTable(
                name: "BANKA");

            migrationBuilder.DropTable(
                name: "GRADA");

            migrationBuilder.DropTable(
                name: "POZITA");

            migrationBuilder.DropTable(
                name: "DEPARTAMENTI");

            migrationBuilder.DropTable(
                name: "KOMPANIA");

            migrationBuilder.DropTable(
                name: "KOMUNA");
        }
    }
}
