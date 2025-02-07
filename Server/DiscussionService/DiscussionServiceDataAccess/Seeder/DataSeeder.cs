using DiscussionServiceDataAccess.DatabaseContext;
using DiscussionServiceDomain.Models;
using Microsoft.Extensions.Logging;

namespace DiscussionServiceDataAccess.Seeder
{
    public class DataSeeder(DiscussionDbContext context, ILogger<DataSeeder> logger)
    {
        private readonly DiscussionDbContext _context = context;
        private readonly ILogger<DataSeeder> _logger = logger;
        public async Task SeedAsync()
        {
            List<Discussion> discussions =
                [
                    new Discussion(
                            "Memento discussion",
                            "First discussion about themes in movie Memento", 
                            DateTime.Parse("2023-09-25"), 
                            Guid.Parse("f530fc7e-fa6d-4b64-b813-04650f61be29"), 
                            true, 
                            [
                                Guid.Parse("f530fc7e-fa6d-4b64-b813-04650f61be29"),
                                Guid.Parse("2de57bf5-c74e-4d2f-b803-0ea8e6dc63ce")
                            ]                            
                        ) 
                        { Id = Guid.NewGuid() },

                    new Discussion(
                            "Harry Potter and the Philosopher's Stone discussion",
                            "Discussion about charachters in movie Harry Potter and the Philosopher's Stone",
                            DateTime.Parse("2024-11-11"),
                            Guid.Parse("2de57bf5-c74e-4d2f-b803-0ea8e6dc63ce"),
                            true,
                            [
                                Guid.Parse("2de57bf5-c74e-4d2f-b803-0ea8e6dc63ce")
                            ]
                        )
                        { Id = Guid.NewGuid() },
                ];

            List<Message> messages =
                [
                    new Message(discussions[0].Id, "Really cool movie", Guid.Parse("f530fc7e-fa6d-4b64-b813-04650f61be29")) { Id = Guid.NewGuid() },
                    new Message(discussions[0].Id, "I didn't get the main theme though", Guid.Parse("2de57bf5-c74e-4d2f-b803-0ea8e6dc63ce")) { Id = Guid.NewGuid() },
                    new Message(discussions[0].Id, "It's okay", Guid.Parse("f530fc7e-fa6d-4b64-b813-04650f61be29")) { Id = Guid.NewGuid() },

                    new Message(discussions[1].Id, "I like the 5th one better", Guid.Parse("f530fc7e-fa6d-4b64-b813-04650f61be29")) { Id = Guid.NewGuid() },
                    new Message(discussions[1].Id, "Me too", Guid.Parse("2de57bf5-c74e-4d2f-b803-0ea8e6dc63ce")) { Id = Guid.NewGuid() },
                    new Message(discussions[1].Id, "I need to rewatch it next", Guid.Parse("f530fc7e-fa6d-4b64-b813-04650f61be29")) { Id = Guid.NewGuid() },
                ];

            _context.Database.AutoTransactionBehavior = Microsoft.EntityFrameworkCore.AutoTransactionBehavior.Never;
            if (!_context.Discussions.Any())
            {
                _context.Discussions.AddRange(discussions);
            }

            if (!_context.Messages.Any())
            {
                _context.Messages.AddRange(messages);
            }

            await _context.SaveChangesAsync();
            _context.Database.AutoTransactionBehavior = Microsoft.EntityFrameworkCore.AutoTransactionBehavior.WhenNeeded;

            _logger.LogInformation("Seeding completed successfully");
        }
    }
}
