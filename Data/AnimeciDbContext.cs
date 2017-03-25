using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AnimeciBackend.Data
{
    public class AnimeciDbContext : IdentityDbContext<ApplicationUser>
    {
        public AnimeciDbContext(DbContextOptions<AnimeciDbContext> options)
            : base(options) { }

        public DbSet<Anime> Anime { get; set; }
        public DbSet<Bolum> Bolum { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<UBolumListe> UBolumListe { get; set; }
        public DbSet<UAnimeListe> UAnimeListe { get; set; }
        public DbSet<QueJobs> QueJobs { get; set; }
        // public DbSet<UpdaterResult> UpdaterResults {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Anime>()
            .ToTable("anime");
            modelBuilder.Entity<Bolum>()
            .ToTable("bolum");
            modelBuilder.Entity<Video>()
            .ToTable("video");
            modelBuilder.Entity<UAnimeListe>()
            .ToTable("uanimeliste");
            modelBuilder.Entity<UBolumListe>()
            .ToTable("ubolumliste");

            modelBuilder.Entity<QueJobs>()
                .ToTable("que_jobs");

            modelBuilder.Entity<QueJobs>()
                .Property(o => o.job_class)
                .IsRequired();

            modelBuilder.Entity<QueJobs>()
                .Property(o => o.run_at)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<QueJobs>()
                .Property(o => o.priority)
                .HasDefaultValueSql("100");

            modelBuilder.Entity<QueJobs>()
                .Property(o => o.error_count)
                .IsRequired()
                .HasDefaultValueSql("0");

            modelBuilder.Entity<QueJobs>()
                .Property(o => o.queue)
                .IsRequired()
                .HasDefaultValueSql("''::text");

            modelBuilder.Entity<QueJobs>()
                .Property(o => o.args)
                .IsRequired()
                .HasColumnType("json")
                .HasDefaultValueSql("'[]'::json");

            modelBuilder.Entity<Anime>()
                .HasAlternateKey(a => a.Taid);

            modelBuilder.Entity<Anime>()
                .Property(o => o.MalGuncel)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Anime>()
                .Property(o => o.Guncel)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Anime>()
                .Property(o => o.Eklendi)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Anime>()
                .Property(o => o.Adi)
                .IsRequired();

            modelBuilder.Entity<Anime>()
                .Property(o => o.Tip)
                .IsRequired();

            modelBuilder.Entity<Anime>()
                .Property(o => o.Tip)
                .HasDefaultValue("TV");

            modelBuilder.Entity<Anime>()
                .Property(o => o.MalID)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<Anime>()
                .Property(o => o.Sure)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<Anime>()
                .Property(o => o.MalID)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<Anime>()
                .Property(o => o.MalInfo)
                .HasDefaultValueSql("false");

            modelBuilder.Entity<Anime>()
                .Property(b => b.Atarashii)
                .HasColumnType("jsonb");

            modelBuilder.Entity<Bolum>()
                .HasAlternateKey(b => b.Bid);

            modelBuilder.Entity<Bolum>()
                .Property(o => o.Adi)
                .IsRequired();

            modelBuilder.Entity<Bolum>()
                .Property(o => o.Guncel)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Bolum>()
            .Property(o => o.Eklendi)
            .HasDefaultValueSql("now()");

            modelBuilder.Entity<Video>()
                .HasAlternateKey(b => b.Vid);

            modelBuilder.Entity<Video>()
                .Property(o => o.QC)
                .HasDefaultValueSql("true");

            modelBuilder.Entity<Video>()
                .Property(o => o.Guncel)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Video>()
            .Property(o => o.Eklendi)
            .HasDefaultValueSql("now()");

            modelBuilder.Entity<Video>()
                .Property(o => o.Grup)
                .IsRequired();

            modelBuilder.Entity<Video>()
                .Property(o => o.Kimin)
                .IsRequired();

            modelBuilder.Entity<Video>()
                .Property(o => o.Src)
                .IsRequired();

            modelBuilder.Entity<UAnimeListe>()
                .Property(o => o.Tarih)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<UBolumListe>()
                .Property(o => o.Tarih)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Anime>()
                .HasMany(i => i.Bolumler)
                .WithOne(i => i.Anime)
                .HasForeignKey(i => i.AnimeID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bolum>()
                .HasMany(i => i.Videolar)
                .WithOne(i => i.Bolum)
                .HasForeignKey(i => i.BolumID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(i => i.AnimeListesi)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(i => i.BolumListesi)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}