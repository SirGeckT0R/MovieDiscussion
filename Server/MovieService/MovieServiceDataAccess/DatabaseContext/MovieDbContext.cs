using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.DatabaseContext
{
    public class MovieDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<Person> People { get; set; } = null!;
        public DbSet<Watchlist> Watchlists { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().ToCollection("genres");
            modelBuilder.Entity<Movie>().ToCollection("movies");
            modelBuilder.Entity<Person>().ToCollection("people");
            modelBuilder.Entity<Watchlist>().ToCollection("watchlists");
            modelBuilder.Entity<Review>().ToCollection("reviews");
        }
    }
}
