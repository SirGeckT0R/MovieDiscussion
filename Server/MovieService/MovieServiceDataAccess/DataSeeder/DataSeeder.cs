using Microsoft.Extensions.Logging;
using MovieServiceDataAccess.DatabaseContext;
using MovieServiceDomain.Enums;
using MovieServiceDomain.Models;
namespace MovieServiceDataAccess.DataSeeder
{
    public class DataSeeder(MovieDbContext context, ILogger<DataSeeder> logger)
    {
        private readonly MovieDbContext _context = context;
        private readonly ILogger<DataSeeder> _logger = logger;
        public async Task SeedAsync()
        {
            List<Genre> genres =
                [
                        new Genre("Action") { Id = Guid.NewGuid() },
                        new Genre("Drama")  { Id = Guid.NewGuid() },
                        new Genre("Comedy") { Id = Guid.NewGuid() },
                        new Genre("Horror")  { Id = Guid.NewGuid() },
                        new Genre("Romance")  { Id = Guid.NewGuid() },
                        new Genre("Sci-Fi")  { Id = Guid.NewGuid() },
                        new Genre("Fantasy")  { Id = Guid.NewGuid() },
                        new Genre("Animation")  { Id = Guid.NewGuid() },
                        new Genre("Crime")  { Id = Guid.NewGuid() },
                        new Genre("Adventure")  { Id = Guid.NewGuid() },
                        new Genre("Mystery")  { Id = Guid.NewGuid() },
                        new Genre("Thriller")  { Id = Guid.NewGuid() },
                        new Genre("Documentary")  { Id = Guid.NewGuid() },
                        new Genre("Family")  { Id = Guid.NewGuid() },
                        new Genre("War")  { Id = Guid.NewGuid() },
                        new Genre("Musical")  { Id = Guid.NewGuid() },
                        new Genre("Biography")  { Id = Guid.NewGuid() },
                        new Genre("History")  { Id = Guid.NewGuid() },
                        new Genre("Sport")  { Id = Guid.NewGuid() },
                        new Genre("Western")  { Id = Guid.NewGuid() }

                ];

            List<Person> people =
                [
                    new Person("Mark", "Hamill", DateTime.Parse("1951-09-25")) { Id = Guid.NewGuid() },
                    new Person("Henry", "Cavill", DateTime.Parse("1983-05-05")) { Id = Guid.NewGuid() },
                    new Person("Emma", "Stone", DateTime.Parse("1988-11-06")) { Id = Guid.NewGuid() },
                    new Person("Daniel", "Radcliffe", DateTime.Parse("1989-07-23")) { Id = Guid.NewGuid() },
                    new Person("Rupert", "Grint", DateTime.Parse("1983-05-05")) { Id = Guid.NewGuid() },    
                    new Person("Emma", "Watson", DateTime.Parse("1990-04-15")) { Id = Guid.NewGuid() },
                    new Person("Christopher", "Columbus", DateTime.Parse("1958-09-10")) { Id = Guid.NewGuid() },
                    new Person("David", "Heyman", DateTime.Parse("1961-07-26")) { Id = Guid.NewGuid() },
                    new Person("Steve", "Kloves", DateTime.Parse("1960-03-18")) { Id = Guid.NewGuid() },
                    new Person("Guy", "Pearce", DateTime.Parse("1967-10-05")) { Id = Guid.NewGuid() },
                    new Person("Christopher", "Nolan", DateTime.Parse("1970-07-30")) { Id = Guid.NewGuid() },    
                    new Person("Jonathan", "Nolan", DateTime.Parse("1976-06-06")) { Id = Guid.NewGuid() },    
                ];

            List<UserProfile> profiles =
                [
                    new UserProfile(Guid.Parse("f530fc7e-fa6d-4b64-b813-04650f61be29"), "FirstUsername")  { Id = Guid.NewGuid() },
                    new UserProfile(Guid.Parse("2de57bf5-c74e-4d2f-b803-0ea8e6dc63ce"), "SecondUsername") { Id = Guid.NewGuid() },
                ];


            List<Movie> movies =
                [
                    new Movie("Harry Potter and the Philosopher's Stone", 
                            "An orphaned boy enrolls in a school of wizardry, where he learns the truth about himself, his family and the terrible evil that haunts the magical world.", 
                            DateTime.Parse("2001-11-04"),
                            profiles[0].Id,
                            [
                                genres[6].Id,
                                genres[9].Id,
                                genres[10].Id,
                            ],
                            [
                                new CrewMember(people[3].Id, CrewRole.Actor),
                                new CrewMember(people[4].Id, CrewRole.Actor),
                                new CrewMember(people[5].Id, CrewRole.Actor),
                                new CrewMember(people[6].Id, CrewRole.Director),
                                new CrewMember(people[7].Id, CrewRole.Producer),
                                new CrewMember(people[8].Id, CrewRole.Screenwriter),
                            ]
                            ) { Id = Guid.NewGuid()},
                    new Movie("Memento",
                            "Leonard Shelby, an insurance investigator, suffers from anterograde amnesia and uses notes and tattoos to hunt for the man he thinks killed his wife, which is the last thing he remembers.",
                            DateTime.Parse("2000-09-05"),
                            profiles[1].Id,
                            [
                                genres[0].Id,
                                genres[1].Id,
                                genres[8].Id,
                                genres[11].Id,
                            ],
                            [
                                new CrewMember(people[9].Id, CrewRole.Actor),
                                new CrewMember(people[10].Id, CrewRole.Director),
                                new CrewMember(people[11].Id, CrewRole.Screenwriter),
                            ]
                            ) { Id = Guid.NewGuid()},

                ];

            List<Review> reviews =
                [
                    new Review(movies[0].Id, profiles[0].Id, 8, "Very Cool") { Id = Guid.NewGuid() },
                    new Review(movies[1].Id, profiles[0].Id, 2, "Not Cool") { Id = Guid.NewGuid() },
                    new Review(movies[0].Id, profiles[1].Id, 4, "Bad") { Id = Guid.NewGuid() },
                    new Review(movies[1].Id, profiles[1].Id, 10, "Masterpiece") { Id = Guid.NewGuid() },
                ];

            List<Watchlist> watchlists =
                [
                    new Watchlist(profiles[0].Id, [
                        movies[0].Id,
                        movies[1].Id,
                    ]) { Id = Guid.NewGuid() },

                    new Watchlist(profiles[1].Id, [
                        movies[1].Id,
                    ]) { Id = Guid.NewGuid() },
                ];

            movies.ForEach(x => x.Rating = reviews.Where(y => y.MovieId == x.Id).Select(y => y.Value).Average());

            _context.Database.AutoTransactionBehavior = Microsoft.EntityFrameworkCore.AutoTransactionBehavior.Never;
            if (!_context.Genres.Any())
            {
                _context.Genres.AddRange(genres);
            }

            if (!_context.People.Any())
            {
                _context.People.AddRange(people);
            }

            if (!_context.UserProfiles.Any())
            {
                _context.UserProfiles.AddRange(profiles);
            }

            if (!_context.Movies.Any())
            {
                _context.Movies.AddRange(movies);
            }

            if (!_context.Watchlists.Any())
            {
                _context.Watchlists.AddRange(watchlists);
            }

            if (!_context.Reviews.Any())
            {
                _context.Reviews.AddRange(reviews);
            }


            await _context.SaveChangesAsync();
            _context.Database.AutoTransactionBehavior = Microsoft.EntityFrameworkCore.AutoTransactionBehavior.WhenNeeded;

            _logger.LogInformation("Seeding completed successfully");
        }
    }
}
