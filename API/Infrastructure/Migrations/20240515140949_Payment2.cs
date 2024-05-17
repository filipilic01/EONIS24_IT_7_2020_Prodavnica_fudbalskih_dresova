using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Payment2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DatumPorudzbine",
                table: "Porudzbinas",
                newName: "DatumKreiranja");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatumAzuriranja",
                table: "Porudzbinas",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatumAzuriranja",
                table: "Porudzbinas");

            migrationBuilder.RenameColumn(
                name: "DatumKreiranja",
                table: "Porudzbinas",
                newName: "DatumPorudzbine");
        }
    }
}
