using MongoDB.Driver;
using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.IndexesConfiguration
{
    public static class WatchlistIndexConfiguration
    {
        public static void CreateIndexes(IMongoCollection<Watchlist> collection)
        {
            var indexKeysDefinition = Builders<Watchlist>.IndexKeys.Ascending(profile => profile.ProfileId);
            var options = new CreateIndexOptions() { Unique = true };
            collection.Indexes.CreateOne(new CreateIndexModel<Watchlist>(indexKeysDefinition, options));
        }
    }
}
