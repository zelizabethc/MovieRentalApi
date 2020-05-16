using Microsoft.EntityFrameworkCore;
using MovieRentalAdminApi.Domain.Entities;

namespace MovieRentalAdminApi.DataAccess
{
    public class MovieRentalDbContext : DbContext
    {
        public MovieRentalDbContext(DbContextOptions<MovieRentalDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MovieLikesEntity>()
                .HasKey(pl => new {pl.MovieId, pl.UserAccountId });

            modelBuilder.Entity<MovieLikesEntity>()
                .HasOne(pl => pl.Movie)
                .WithMany(p => p.UserAccountLikes)
                .HasForeignKey(a => a.MovieId);

            modelBuilder.Entity<MovieLikesEntity>()
                .HasOne(pl => pl.UserAccount)
                .WithMany(p => p.LikedMovies)
                .HasForeignKey(a => a.UserAccountId);

            modelBuilder.Entity<ImageEntity>()
                .HasKey(pl => new { pl.Id, pl.MovieId });

            modelBuilder.Entity<ImageEntity>()
                .HasOne(pl => pl.Movie)
                .WithMany(p => p.Images)
                .HasForeignKey(a => a.MovieId);

            modelBuilder.Entity<MovieEntity>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<MovieUpdateLogEntity>()
               .Property(p => p.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<RentedTrackingEntity>()
               .Property(p => p.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<MovieActionLogEntity>()
               .Property(p => p.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<RentalSettingsEntity>()
               .Property(p => p.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserAccountEntity>()
               .Property(p => p.Id)
               .ValueGeneratedOnAdd();
        }

        public DbSet<MovieEntity> Movie { get; set; }
        public DbSet<UserAccountEntity> UserAccount { get; set; }
        public DbSet<MovieLikesEntity> MovieLikes { get; set; }
        public DbSet<ImageEntity> Image { get; set; }
        public DbSet<MovieUpdateLogEntity> MovieUpdateLog { get; set; }
        public DbSet<RentedTrackingEntity> RentedTracking { get; set; }
        public DbSet<MovieActionLogEntity> MovieActionLog { get; set; }
        public DbSet<RentalSettingsEntity> RentalSettings { get; set; }

    }
}
