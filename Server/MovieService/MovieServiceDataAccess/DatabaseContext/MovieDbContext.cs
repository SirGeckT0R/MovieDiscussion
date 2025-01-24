using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using MovieServiceDataAccess.IndexesConfiguration;
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
        public DbSet<UserProfile> UserProfiles { get; set; } = null!;

        private readonly IConfiguration _configuration;
        public MovieDbContext(DbContextOptions<MovieDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().ToCollection("genres");
            modelBuilder.Entity<Movie>().ToCollection("movies");
            modelBuilder.Entity<Person>().ToCollection("people");
            modelBuilder.Entity<Watchlist>().ToCollection("watchlists");
            modelBuilder.Entity<Review>().ToCollection("reviews");
            modelBuilder.Entity<UserProfile>().ToCollection("userProfiles");

            BuildIndexes();
        }

        private void BuildIndexes()
        {
            var client = new MongoClient(_configuration["MovieDbConnection"]!);
            var database = client.GetDatabase(_configuration["MovieDbName"]!);
            var profileCollection = database.GetCollection<UserProfile>("userProfiles");
            var genreCollection = database.GetCollection<Genre>("genres");
            var watchlistCollection = database.GetCollection<Watchlist>("watchlists");
            var reviewCollection = database.GetCollection<Review>("reviews");

            UserProfileIndexConfiguration.CreateIndexes(profileCollection);
            GenreIndexConfiguration.CreateIndexes(genreCollection);
            WatchlistIndexConfiguration.CreateIndexes(watchlistCollection);
            ReviewIndexConfiguration.CreateIndexes(reviewCollection);
        }
    }
}
