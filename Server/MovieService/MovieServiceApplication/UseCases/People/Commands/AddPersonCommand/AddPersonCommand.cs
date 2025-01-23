using MediatR;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.People.Commands.AddPersonCommand
{
    public record AddPersonCommand(string FirstName, string LastName, DateTime DateOfBirth) : ICommand<Unit>;
}
