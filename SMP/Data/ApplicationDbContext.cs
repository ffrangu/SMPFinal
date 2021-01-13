using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SMP.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public virtual DbSet<Banka> Banka { get; set; }
        public virtual DbSet<Bonuset> Bonuset { get; set; }
        public virtual DbSet<Departamenti> Departamenti { get; set; }
        public virtual DbSet<Grada> Grada { get; set; }
        public virtual DbSet<Kompania> Kompania { get; set; }
        public virtual DbSet<Komuna> Komuna { get; set; }
        public virtual DbSet<LogUseractivity> LogUseractivity { get; set; }
        public virtual DbSet<Paga> Paga { get; set; }
        public virtual DbSet<Pozita> Pozita { get; set; }
        public virtual DbSet<Punetori> Punetori { get; set; }
        public virtual DbSet<PunetoriKontrata> PunetoriKontrata { get; set; }
        public virtual DbSet<Tatimi> Tatimi { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Data Source=34.107.3.230;Initial Catalog=SMP;user=sqlserver;password=D@ta123;");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Banka>(entity =>
            {
                entity.ToTable("BANKA");

                entity.Property(e => e.Emri)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Kodi)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Bonuset>(entity =>
            {
                entity.ToTable("BONUSET");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Pershkrimi)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Vlera).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Punetori)
                    .WithMany(p => p.Bonuset)
                    .HasForeignKey(d => d.PunetoriId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BONUSET_PUNETORI");
            });

            modelBuilder.Entity<Departamenti>(entity =>
            {
                entity.ToTable("DEPARTAMENTI");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Emri)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Shkurtesa)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Kompania)
                    .WithMany(p => p.Departamenti)
                    .HasForeignKey(d => d.KompaniaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DEPARTAMENTI_KOMPANIA");
            });

            modelBuilder.Entity<Grada>(entity =>
            {
                entity.ToTable("GRADA");

                entity.Property(e => e.Emri)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PagaMujore).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PagaVjetore).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Kompania>(entity =>
            {
                entity.ToTable("KOMPANIA");

                entity.Property(e => e.Emri)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Kodi)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Komuna)
                    .WithMany(p => p.Kompania)
                    .HasForeignKey(d => d.KomunaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KOMPANIA_KOMUNA");
            });

            modelBuilder.Entity<Komuna>(entity =>
            {
                entity.ToTable("KOMUNA");

                entity.Property(e => e.Emri)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<LogUseractivity>(entity =>
            {
                entity.ToTable("LOG_USERACTIVITY");

                entity.Property(e => e.Activity)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.EntryDate).HasColumnType("datetime");

                entity.Property(e => e.HttpMethod)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Paga>(entity =>
            {
                entity.ToTable("PAGA");

                entity.Property(e => e.Bonuse).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.BonuseNeto).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Bruto).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.DataEkzekutimit).HasColumnType("datetime");

                entity.Property(e => e.KontributiPunedhenesi).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.KontributiPunetori).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PagaFinale).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PagaNeto).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PagaTatim).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Tatimi).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Grada)
                    .WithMany(p => p.Paga)
                    .HasForeignKey(d => d.GradaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGA_GRADA");

                entity.HasOne(d => d.Punetori)
                    .WithMany(p => p.Paga)
                    .HasForeignKey(d => d.PunetoriId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PAGA_PUNETORI");
            });

            modelBuilder.Entity<Pozita>(entity =>
            {
                entity.ToTable("POZITA");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Emri)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Departamenti)
                    .WithMany(p => p.Pozita)
                    .HasForeignKey(d => d.DepartamentiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POZITA_DEPARTAMENTI");

                entity.HasOne(d => d.Kompania)
                    .WithMany(p => p.Pozita)
                    .HasForeignKey(d => d.KompaniaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POZITA_KOMPANIA");
            });

            modelBuilder.Entity<Punetori>(entity =>
            {
                entity.ToTable("PUNETORI");

                entity.Property(e => e.Adresa).HasMaxLength(250);

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Datelindja).HasColumnType("datetime");

                entity.Property(e => e.Emri)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Mbiemri)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.NumriPersonal)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.Property(e => e.Xhirollogaria)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.HasOne(d => d.Banka)
                    .WithMany(p => p.Punetori)
                    .HasForeignKey(d => d.BankaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PUNETORI_BANKA");

                entity.HasOne(d => d.Departamenti)
                    .WithMany(p => p.Punetori)
                    .HasForeignKey(d => d.DepartamentiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PUNETORI_DEPARTAMENTI");

                entity.HasOne(d => d.Grada)
                    .WithMany(p => p.Punetori)
                    .HasForeignKey(d => d.GradaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PUNETORI_GRADA");

                entity.HasOne(d => d.Kompania)
                    .WithMany(p => p.Punetori)
                    .HasForeignKey(d => d.KompaniaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PUNETORI_KOMPANIA");

                entity.HasOne(d => d.Komuna)
                    .WithMany(p => p.Punetori)
                    .HasForeignKey(d => d.KomunaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PUNETORI_KOMUNA");

                entity.HasOne(d => d.Pozita)
                    .WithMany(p => p.Punetori)
                    .HasForeignKey(d => d.PozitaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PUNETORI_POZITA");
            });

            modelBuilder.Entity<PunetoriKontrata>(entity =>
            {
                entity.ToTable("PUNETORI_KONTRATA");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Emri)
                    .IsRequired()
                    .HasColumnType("nvarchar(200)");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PunetoriId);

                entity.HasOne(d => d.Punetori)
                    .WithMany(p => p.PunetoriKontrata)
                    .HasForeignKey(d => d.PunetoriId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PUNETORI_KONTRATA_PUNETORI");
            });

            modelBuilder.Entity<Tatimi>(entity =>
            {
                entity.ToTable("TATIMI");

                entity.Property(e => e.PerqindjaDyte).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PerqindjaPare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PerqindjaTrete).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PerqindjaZero).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VleraDyte).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VleraPare).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VleraTrete).HasColumnType("decimal(18, 2)");
            });

            //OnModelCreatingPartial(modelBuilder);
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
