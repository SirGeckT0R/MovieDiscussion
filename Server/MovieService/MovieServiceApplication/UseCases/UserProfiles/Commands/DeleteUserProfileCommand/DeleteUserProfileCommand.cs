using MediatR;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.UserProfiles.Commands.DeleteUserProfileCommand
{
    public record DeleteUserProfileCommand(Guid AccountId) : ICommand<Unit>;
}
