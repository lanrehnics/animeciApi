using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnimeciBackend.Migrations
{
    public partial class eklendi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Eklendi",
                table: "video",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Eklendi",
                table: "bolum",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Eklendi",
                table: "anime",
                nullable: false,
                defaultValueSql: "now()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Eklendi",
                table: "video");

            migrationBuilder.DropColumn(
                name: "Eklendi",
                table: "bolum");

            migrationBuilder.DropColumn(
                name: "Eklendi",
                table: "anime");
        }
    }
}
