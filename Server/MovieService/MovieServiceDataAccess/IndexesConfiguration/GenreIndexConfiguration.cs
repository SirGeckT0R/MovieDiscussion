using MongoDB.Driver;
using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.IndexesConfiguration
{
    public static class GenreIndexConfiguration
    {
        public static void CreateIndexes(IMongoCollection<Genre> collection)
        {
            var indexKeysDefinition = Builders<Genre>.IndexKeys.Ascending(profile => profile.Name);
            var options = new CreateIndexOptions() { Unique = true };
            collection.Indexes.CreateOne(new CreateIndexModel<Genre>(indexKeysDefinition, options));
        }
    }
}
