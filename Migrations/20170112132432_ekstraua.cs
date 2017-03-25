using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnimeciBackend.Migrations
{
    public partial class ekstraua : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ekstra",
                table: "uanimeliste",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ubolumliste_BolumId",
                table: "ubolumliste",
                column: "BolumId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_uanimeliste_AnimeId",
                table: "uanimeliste",
                column: "AnimeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_uanimeliste_anime_AnimeId",
                table: "uanimeliste",
                column: "AnimeId",
                principalTable: "anime",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ubolumliste_bolum_BolumId",
                table: "ubolumliste",
                column: "BolumId",
                principalTable: "bolum",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_uanimeliste_anime_AnimeId",
                table: "uanimeliste");

            migrationBuilder.DropForeignKey(
                name: "FK_ubolumliste_bolum_BolumId",
                table: "ubolumliste");

            migrationBuilder.DropIndex(
                name: "IX_ubolumliste_BolumId",
                table: "ubolumliste");

            migrationBuilder.DropIndex(
                name: "IX_uanimeliste_AnimeId",
                table: "uanimeliste");

            migrationBuilder.DropColumn(
                name: "Ekstra",
                table: "uanimeliste");
        }
    }
}
