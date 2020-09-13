using FootballManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.Data
{
    public partial class FootballManagerContext : DbContext
    {
        public FootballManagerContext()
        {
        }

        public FootballManagerContext(DbContextOptions<FootballManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Club> Clubs { get; set; }
        public virtual DbSet<Coach> Coaches { get; set; }
        public virtual DbSet<Footballer> Footballers { get; set; }
        public virtual DbSet<Stadium> Stadiums { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coach>(entity =>
            {
                entity.HasIndex(e => e.ClubId)
                    .IsUnique();

                entity.HasOne(d => d.Club)
                    .WithOne(p => p.Coach)
                    .HasForeignKey<Coach>(d => d.ClubId);
            });

            modelBuilder.Entity<Footballer>(entity =>
            {
                entity.HasIndex(e => e.ClubId);

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.Footballers)
                    .HasForeignKey(d => d.ClubId);
            });

            modelBuilder.Entity<Stadium>(entity =>
            {
                entity.HasIndex(e => e.ClubId)
                    .IsUnique();

                entity.HasOne(d => d.Club)
                    .WithOne(p => p.Stadium)
                    .HasForeignKey<Stadium>(d => d.ClubId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}