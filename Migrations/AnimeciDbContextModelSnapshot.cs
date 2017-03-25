using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AnimeciBackend.Data;

namespace AnimeciBackend.Migrations
{
    [DbContext(typeof(AnimeciDbContext))]
    partial class AnimeciDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("AnimeciBackend.Data.Anime", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("Adi")
                        .IsRequired();

                    b.Property<string>("Alternatif");

                    b.Property<string>("Atarashii")
                        .HasColumnType("jsonb");

                    b.Property<string>("BolumSayisi");

                    b.Property<DateTimeOffset>("Eklendi")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<DateTimeOffset>("Guncel")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<DateTimeOffset>("MalGuncel")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<int>("MalID")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<bool>("MalInfo")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("false");

                    b.Property<string>("Ozet");

                    b.Property<string>("Poster");

                    b.Property<int>("Sure")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<int>("Taid");

                    b.Property<string>("Tip")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("TV");

                    b.Property<string[]>("Turleri");

                    b.Property<string>("URL");

                    b.Property<string>("Yili");

                    b.HasKey("ID");

                    b.HasAlternateKey("Taid");

                    b.ToTable("anime");
                });

            modelBuilder.Entity("AnimeciBackend.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("AnimeciBackend.Data.Bolum", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("Adi")
                        .IsRequired();

                    b.Property<int>("AnimeID");

                    b.Property<long>("Bid");

                    b.Property<DateTimeOffset>("Eklendi")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<DateTimeOffset>("Guncel")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<string>("URL");

                    b.HasKey("ID");

                    b.HasAlternateKey("Bid");

                    b.HasIndex("AnimeID");

                    b.ToTable("bolum");
                });

            modelBuilder.Entity("AnimeciBackend.Data.QueJobs", b =>
                {
                    b.Property<long>("job_id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("args")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("json")
                        .HasDefaultValueSql("'[]'::json");

                    b.Property<int>("error_count")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<string>("job_class")
                        .IsRequired();

                    b.Property<string>("last_error");

                    b.Property<byte>("priority")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("100");

                    b.Property<string>("queue")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("''::text");

                    b.Property<DateTimeOffset>("run_at")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.HasKey("job_id");

                    b.ToTable("que_jobs");
                });

            modelBuilder.Entity("AnimeciBackend.Data.UAnimeListe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("AnimeId");

                    b.Property<int>("Ekstra");

                    b.Property<string>("Kaynak");

                    b.Property<int>("ListType");

                    b.Property<int>("Puan");

                    b.Property<DateTimeOffset>("Tarih")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("AnimeId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("uanimeliste");
                });

            modelBuilder.Entity("AnimeciBackend.Data.UBolumListe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("AnimeId");

                    b.Property<int>("BolumId");

                    b.Property<long>("Ekstra");

                    b.Property<int>("Puan");

                    b.Property<DateTimeOffset>("Tarih")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("BolumId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("ubolumliste");
                });

            modelBuilder.Entity("AnimeciBackend.Data.Video", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("BolumID");

                    b.Property<DateTimeOffset>("Eklendi")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<string>("EmegiGecenler");

                    b.Property<string>("Grup")
                        .IsRequired();

                    b.Property<DateTimeOffset>("Guncel")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Kimin")
                        .IsRequired();

                    b.Property<string>("Nerden");

                    b.Property<bool>("QC")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("true");

                    b.Property<string>("Src")
                        .IsRequired();

                    b.Property<int>("Vid");

                    b.HasKey("ID");

                    b.HasAlternateKey("Vid");

                    b.HasIndex("BolumID");

                    b.ToTable("video");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("AnimeciBackend.Data.Bolum", b =>
                {
                    b.HasOne("AnimeciBackend.Data.Anime", "Anime")
                        .WithMany("Bolumler")
                        .HasForeignKey("AnimeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AnimeciBackend.Data.UAnimeListe", b =>
                {
                    b.HasOne("AnimeciBackend.Data.Anime", "Anime")
                        .WithOne("Liste")
                        .HasForeignKey("AnimeciBackend.Data.UAnimeListe", "AnimeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AnimeciBackend.Data.ApplicationUser", "User")
                        .WithMany("AnimeListesi")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AnimeciBackend.Data.UBolumListe", b =>
                {
                    b.HasOne("AnimeciBackend.Data.Bolum")
                        .WithOne("Liste")
                        .HasForeignKey("AnimeciBackend.Data.UBolumListe", "BolumId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AnimeciBackend.Data.ApplicationUser", "User")
                        .WithMany("BolumListesi")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AnimeciBackend.Data.Video", b =>
                {
                    b.HasOne("AnimeciBackend.Data.Bolum", "Bolum")
                        .WithMany("Videolar")
                        .HasForeignKey("BolumID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AnimeciBackend.Data.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AnimeciBackend.Data.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AnimeciBackend.Data.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
