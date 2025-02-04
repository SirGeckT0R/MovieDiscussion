using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.People.Commands.UpdatePersonCommand
{
    public class UpdatePersonHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdatePersonHandler> logger) : ICommandHandler<UpdatePersonCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdatePersonHandler> _logger = logger;

        public async Task<Unit> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            var candidatePerson = await _unitOfWork.People.GetByIdAsync(request.Id, cancellationToken);

            if (candidatePerson == null)
            {
                _logger.LogError("Update person command failed: person not found");

                throw new NotFoundException("Person not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var person = _mapper.Map<Person>(request);
            _unitOfWork.People.Update(person, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
