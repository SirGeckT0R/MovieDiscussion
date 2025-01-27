using MediatR;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.People.Commands.DeletePersonCommand
{
    public record DeletePersonCommand(Guid Id) : ICommand<Unit>;
}
