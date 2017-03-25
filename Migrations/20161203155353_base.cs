using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AnimeciBackend.Migrations
{
    public partial class @base : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "anime",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Adi = table.Column<string>(nullable: false),
                    Alternatif = table.Column<string>(nullable: true),
                    Atarashii = table.Column<string>(type: "jsonb", nullable: true),
                    BolumSayisi = table.Column<string>(nullable: true),
                    Guncel = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "now()"),
                    MalGuncel = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "now()"),
                    MalID = table.Column<int>(nullable: false, defaultValueSql: "0"),
                    MalInfo = table.Column<bool>(nullable: false, defaultValueSql: "false"),
                    Ozet = table.Column<string>(nullable: true),
                    Poster = table.Column<string>(nullable: true),
                    Sure = table.Column<int>(nullable: false, defaultValueSql: "0"),
                    Taid = table.Column<int>(nullable: false),
                    Tip = table.Column<string>(nullable: false, defaultValue: "TV"),
                    Turleri = table.Column<List<string>>(nullable: true),
                    URL = table.Column<string>(nullable: true),
                    Yili = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_anime", x => x.ID);
                    table.UniqueConstraint("AK_anime_Taid", x => x.Taid);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "que_jobs",
                columns: table => new
                {
                    job_id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    args = table.Column<string>(type: "json", nullable: false, defaultValueSql: "'[]'::json"),
                    error_count = table.Column<int>(nullable: false, defaultValueSql: "0"),
                    job_class = table.Column<string>(nullable: false),
                    last_error = table.Column<string>(nullable: true),
                    priority = table.Column<byte>(nullable: false, defaultValueSql: "100"),
                    queue = table.Column<string>(nullable: false, defaultValueSql: "''::text"),
                    run_at = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_que_jobs", x => x.job_id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "bolum",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Adi = table.Column<string>(nullable: false),
                    AnimeID = table.Column<int>(nullable: false),
                    Bid = table.Column<long>(nullable: false),
                    Guncel = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "now()"),
                    URL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bolum", x => x.ID);
                    table.UniqueConstraint("AK_bolum_Bid", x => x.Bid);
                    table.ForeignKey(
                        name: "FK_bolum_anime_AnimeID",
                        column: x => x.AnimeID,
                        principalTable: "anime",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "uanimeliste",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AnimeId = table.Column<int>(nullable: false),
                    Kaynak = table.Column<string>(nullable: true),
                    ListType = table.Column<int>(nullable: false),
                    Puan = table.Column<int>(nullable: false),
                    Tarih = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "now()"),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uanimeliste", x => x.Id);
                    table.ForeignKey(
                        name: "FK_uanimeliste_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ubolumliste",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AnimeId = table.Column<int>(nullable: false),
                    BolumId = table.Column<int>(nullable: false),
                    Ekstra = table.Column<long>(nullable: false),
                    Puan = table.Column<int>(nullable: false),
                    Tarih = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "now()"),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ubolumliste", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ubolumliste_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "video",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    BolumID = table.Column<int>(nullable: false),
                    EmegiGecenler = table.Column<string>(nullable: true),
                    Grup = table.Column<string>(nullable: false),
                    Guncel = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "now()"),
                    Kimin = table.Column<string>(nullable: false),
                    Nerden = table.Column<string>(nullable: true),
                    QC = table.Column<bool>(nullable: false, defaultValueSql: "true"),
                    Src = table.Column<string>(nullable: false),
                    Vid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_video", x => x.ID);
                    table.UniqueConstraint("AK_video_Vid", x => x.Vid);
                    table.ForeignKey(
                        name: "FK_video_bolum_BolumID",
                        column: x => x.BolumID,
                        principalTable: "bolum",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bolum_AnimeID",
                table: "bolum",
                column: "AnimeID");

            migrationBuilder.CreateIndex(
                name: "IX_uanimeliste_UserId",
                table: "uanimeliste",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ubolumliste_UserId",
                table: "ubolumliste",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_video_BolumID",
                table: "video",
                column: "BolumID");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "que_jobs");

            migrationBuilder.DropTable(
                name: "uanimeliste");

            migrationBuilder.DropTable(
                name: "ubolumliste");

            migrationBuilder.DropTable(
                name: "video");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "bolum");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "anime");
        }
    }
}
