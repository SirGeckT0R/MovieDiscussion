using MediatR;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Movies.Commands.AddMovieCommand
{
    public record AddMovieCommand : ICommand<Unit>
    {
        public string Title { get;  set; } = string.Empty;
        public string Description { get;  set; } = string.Empty;
        public DateTime ReleaseDate { get;  set; } = DateTime.UtcNow;
        public ICollection<Guid> Genres { get;  set; } = [];
        public ICollection<CrewMemberDto> CrewMembers { get;  set; } = [];
        public Guid AccountId { get;  set; }
        public string? Image { get;  set; }

        public AddMovieCommand() { }
    }
}
