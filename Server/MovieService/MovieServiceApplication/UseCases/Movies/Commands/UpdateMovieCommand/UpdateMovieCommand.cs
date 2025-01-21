using MediatR;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Movies.Commands.UpdateMovieCommand
{
    public record UpdateMovieCommand : ICommand<Unit>
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; } = DateTime.UtcNow;
        public ICollection<Guid> Genres { get; set; } = [];
        public ICollection<CrewMemberDto> CrewMembers { get; set; } = [];
    }
}
