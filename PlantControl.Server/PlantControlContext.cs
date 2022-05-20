using Microsoft.EntityFrameworkCore;
using PlantControl.Models;

namespace PlantControl.Server
{
    public partial class PlantControlContext : DbContext
    {
        public PlantControlContext()
        {
        }

        public PlantControlContext(DbContextOptions<PlantControlContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Certificate> Certificates { get; set; } = null!;
        public virtual DbSet<Log> Logs { get; set; } = null!;
        public virtual DbSet<Logger> Loggers { get; set; } = null!;
        public virtual DbSet<Pairing> Pairings { get; set; } = null!;
        public virtual DbSet<Plant> Plants { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Certificate>(entity =>
            {
                entity.ToTable("Certificate");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Pairing)
                    .WithMany(p => p.Certificates)
                    .HasForeignKey(d => d.PairingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PK_Pairing_Certificate");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("Log");

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.Pairing)
                    .WithMany(p => p.Logs)
                    .HasForeignKey(d => d.PairingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PK_Pairing_Log");
            });

            modelBuilder.Entity<Logger>(entity =>
            {
                entity.ToTable("Logger");

                entity.Property(e => e.IsPaired)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Pairing>(entity =>
            {
                entity.ToTable("Pairing");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Logger)
                    .WithMany(p => p.Pairings)
                    .HasForeignKey(d => d.LoggerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Logger_Pairing");

                entity.HasOne(d => d.Plant)
                    .WithMany(p => p.Pairings)
                    .HasForeignKey(d => d.PlantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Plant_Pairing");
            });

            modelBuilder.Entity<Plant>(entity =>
            {
                entity.ToTable("Plant");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Image).HasColumnType("varbinary(max)");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}