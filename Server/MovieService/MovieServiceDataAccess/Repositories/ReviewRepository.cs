using MovieServiceDataAccess.DatabaseContext;
using MovieServiceDataAccess.Interfaces.Repositories;
using MovieServiceDomain.Models;
namespace MovieServiceDataAccess.Repositories
{
    public class ReviewRepository(MovieDbContext dbContext) : BaseRepository<Review>(dbContext), IReviewRepository
    {
    }
}
