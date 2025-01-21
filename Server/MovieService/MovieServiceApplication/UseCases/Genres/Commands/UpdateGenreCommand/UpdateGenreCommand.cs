using MediatR;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Genres.Commands.UpdateGenreCommand
{
    public record UpdateGenreCommand : ICommand<Unit>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public UpdateGenreCommand() { }

        public UpdateGenreCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
