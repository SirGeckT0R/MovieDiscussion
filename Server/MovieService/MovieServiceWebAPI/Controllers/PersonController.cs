using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieServiceApplication.UseCases.People.Commands.AddPersonCommand;
using MovieServiceApplication.UseCases.People.Commands.DeletePersonCommand;
using MovieServiceApplication.UseCases.People.Commands.UpdatePersonCommand;
using MovieServiceApplication.UseCases.People.Queries.GetAllPeopleQuery;
using MovieServiceApplication.UseCases.People.Queries.GetPeopleByNameQuery;
using MovieServiceApplication.UseCases.People.Queries.GetPersonByIdQuery;

namespace MovieServiceWebAPI.Controllers
{
    [ApiController]
    [Route("/api/people")]
    public class PersonController(IMediator mediator, ILogger<PersonController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<PersonController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetPeopleQuery query, CancellationToken cancellationToken)
        {
            var people = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Returning people by query parameters");

            return Ok(people);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AddPersonCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Person was created");

            return Created();
        }

        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] GetPersonByIdQuery query, CancellationToken cancellationToken)
        {
            var person = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Returning a person by id");

            return Ok(person);
        }

        [HttpPut("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromForm] UpdatePersonCommand command, CancellationToken cancellationToken)
        {
            var newCommand = command with { Id = Id };

            await _mediator.Send(newCommand, cancellationToken);

            _logger.LogInformation("Person was updated");

            return NoContent();
        }

        [HttpDelete("{Id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] DeletePersonCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Person was deleted");

            return NoContent();
        }
    }
}
