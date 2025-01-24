using MongoDB.Driver;
using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.IndexesConfiguration
{
    public static class UserProfileIndexConfiguration
    {
        public static void CreateIndexes(IMongoCollection<UserProfile> collection)
        {
            var indexKeysDefinition = Builders<UserProfile>.IndexKeys.Ascending(profile => profile.AccountId);
            var options = new CreateIndexOptions() { Unique = true };
            collection.Indexes.CreateOne(new CreateIndexModel<UserProfile>(indexKeysDefinition, options));
        }
    }
}
