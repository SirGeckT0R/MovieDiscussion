using MongoDB.Driver;
using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.IndexesConfiguration
{
    public static class ReviewIndexConfiguration
    {
        public static void CreateIndexes(IMongoCollection<Review> collection)
        {
            var indexKeysDefinition = Builders<Review>.IndexKeys.Ascending(profile => profile.MovieId).Ascending(profile => profile.ProfileId);
            var options = new CreateIndexOptions() { Unique = true };
            collection.Indexes.CreateOne(new CreateIndexModel<Review>(indexKeysDefinition, options));
        }
    }
}
