using MovieServiceDataAccess.DatabaseContext;
using MovieServiceDataAccess.Interfaces.Repositories;
using MovieServiceDataAccess.Interfaces.UnitOfWork;

namespace MovieServiceDataAccess.UnitOfWork
{
    public class MovieUnitOfWork(MovieDbContext context, 
                                 IMovieRepository movieRepository, 
                                 IGenreRepository genreRepository, 
                                 IPersonRepository personRepository,
                                 IWatchlistRepository watchlistRepository,
                                 IReviewRepository reviewRepository,
                                 IUserProfileRepository userProfileRepository) : IUnitOfWork
    {
        private readonly MovieDbContext _context = context;
        private readonly IMovieRepository _movieRepository = movieRepository;
        private readonly IGenreRepository _genreRepository = genreRepository;
        private readonly IPersonRepository _personRepository = personRepository;
        private readonly IWatchlistRepository _watchlistRepository = watchlistRepository;
        private readonly IReviewRepository _reviewRepository = reviewRepository;
        private readonly IUserProfileRepository _userProfileRepository = userProfileRepository;

        private bool disposed = false;
        public IMovieRepository Movies {
            get 
            { 
                return _movieRepository; 
            }
        }

        public IGenreRepository Genres
        {
            get
            {
                return _genreRepository;
            }
        }

        public IPersonRepository People
        {
            get
            {
                return _personRepository;
            }
        }

        public IWatchlistRepository Watchlists
        {
            get
            {
                return _watchlistRepository;
            }
        }

        public IReviewRepository Reviews
        {
            get
            {
                return _reviewRepository;
            }
        }

        public IUserProfileRepository UserProfiles
        {
            get
            {
                return _userProfileRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                _context.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
