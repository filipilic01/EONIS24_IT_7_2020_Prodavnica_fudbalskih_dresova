using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdminKorisnickoIme = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AdminIme = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AdminPrezime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AdminLozinka = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    AdminEmail = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AdminBrojTelefona = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AdminAdresa = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "Kupacs",
                columns: table => new
                {
                    KupacId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KupacKorisnickoIme = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    KupacIme = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    KupacPrezime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    KupacLozinka = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    KupacEmail = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    KupacBrojTelefona = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    KupacAdresa = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kupacs", x => x.KupacId);
                });

            migrationBuilder.CreateTable(
                name: "Dress",
                columns: table => new
                {
                    DresId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImeIgraca = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Tim = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Sezona = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Brend = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Cena = table.Column<double>(type: "float", nullable: false),
                    SlikaUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Tip = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Zemlja = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Takmicenje = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TimUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dress", x => x.DresId);
                    table.CheckConstraint("CHK_Status", "Status IN ('na stanju', 'nedostupno')");
                    table.ForeignKey(
                        name: "FK_Dress_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "AdminId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Porudzbinas",
                columns: table => new
                {
                    PorudzbinaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UkupanIznos = table.Column<double>(type: "float", nullable: false),
                    DatumPorudzbine = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KupacId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Porudzbinas", x => x.PorudzbinaId);
                    table.ForeignKey(
                        name: "FK_Porudzbinas_Kupacs_KupacId",
                        column: x => x.KupacId,
                        principalTable: "Kupacs",
                        principalColumn: "KupacId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VelicinaDresas",
                columns: table => new
                {
                    VelicinaDresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VelicinaDresaVrednost = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Kolicina = table.Column<int>(type: "int", nullable: false),
                    DresId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VelicinaDresas", x => x.VelicinaDresaId);
                    table.ForeignKey(
                        name: "FK_VelicinaDresas_Dress_DresId",
                        column: x => x.DresId,
                        principalTable: "Dress",
                        principalColumn: "DresId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StavkaPorudzbines",
                columns: table => new
                {
                    StavkaPorudzbineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrojStavki = table.Column<int>(type: "int", nullable: false),
                    PorudzbinaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VelicinaDresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StavkaPorudzbines", x => x.StavkaPorudzbineId);
                    table.ForeignKey(
                        name: "FK_StavkaPorudzbines_Porudzbinas_PorudzbinaId",
                        column: x => x.PorudzbinaId,
                        principalTable: "Porudzbinas",
                        principalColumn: "PorudzbinaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StavkaPorudzbines_VelicinaDresas_VelicinaDresaId",
                        column: x => x.VelicinaDresaId,
                        principalTable: "VelicinaDresas",
                        principalColumn: "VelicinaDresaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_AdminEmail",
                table: "Admins",
                column: "AdminEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admins_AdminKorisnickoIme",
                table: "Admins",
                column: "AdminKorisnickoIme",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dress_AdminId",
                table: "Dress",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Kupacs_KupacEmail",
                table: "Kupacs",
                column: "KupacEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kupacs_KupacKorisnickoIme",
                table: "Kupacs",
                column: "KupacKorisnickoIme",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Porudzbinas_KupacId",
                table: "Porudzbinas",
                column: "KupacId");

            migrationBuilder.CreateIndex(
                name: "IX_StavkaPorudzbines_PorudzbinaId",
                table: "StavkaPorudzbines",
                column: "PorudzbinaId");

            migrationBuilder.CreateIndex(
                name: "IX_StavkaPorudzbines_VelicinaDresaId",
                table: "StavkaPorudzbines",
                column: "VelicinaDresaId");

            migrationBuilder.CreateIndex(
                name: "IX_VelicinaDresas_DresId",
                table: "VelicinaDresas",
                column: "DresId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StavkaPorudzbines");

            migrationBuilder.DropTable(
                name: "Porudzbinas");

            migrationBuilder.DropTable(
                name: "VelicinaDresas");

            migrationBuilder.DropTable(
                name: "Kupacs");

            migrationBuilder.DropTable(
                name: "Dress");

            migrationBuilder.DropTable(
                name: "Admins");
        }
    }
}
