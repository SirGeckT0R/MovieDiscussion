using MediatR;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.UserProfiles.Commands.UpdateUserProfileCommand
{
    public record UpdateUserProfileCommand : ICommand<Unit>
    {
        public Guid AccountId { get; set; }
        public string Username { get; set; } = string.Empty;

        public UpdateUserProfileCommand() { }
    }
}
