using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieServiceApplication.UseCases.People.Commands.DeletePersonCommand
{
    public record DeletePersonCommand(Guid Id) : ICommand<Unit>;
}
