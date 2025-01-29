using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using MovieServiceDataAccess.IndexesConfiguration;
using MovieServiceDomain.Interfaces;
using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.DatabaseContext
{
    public class MovieDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; } = null!;
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

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                if (entry is not { State: EntityState.Deleted, Entity: ISoftDelete delete })
                {
                    continue;
                }

                entry.State = EntityState.Modified;
                delete.IsDeleted = true;
                delete.DeletedAt = DateTime.UtcNow;
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().ToCollection("genres").HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Movie>().ToCollection("movies").HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Person>().ToCollection("people").HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<UserProfile>().ToCollection("userProfiles").HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Watchlist>().ToCollection("watchlists");
            modelBuilder.Entity<Review>().ToCollection("reviews");

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
