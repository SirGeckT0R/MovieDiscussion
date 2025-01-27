using MediatR;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.UserProfiles.Commands.CreateUserProfileCommand
{
    public record CreateUserProfileCommand : ICommand<Unit>
    {
        public Guid AccountId { get; set; }
        public string Username { get; set; } = string.Empty;

        public CreateUserProfileCommand() { }
    }
}