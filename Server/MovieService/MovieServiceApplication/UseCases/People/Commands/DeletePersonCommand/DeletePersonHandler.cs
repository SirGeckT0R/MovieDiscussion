using MediatR;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.People.Commands.DeletePersonCommand
{
    public class DeletePersonHandler(IUnitOfWork unitOfWork, ILogger<DeletePersonHandler> logger) : ICommandHandler<DeletePersonCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<DeletePersonHandler> _logger = logger;

        public async Task<Unit> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            var person = await _unitOfWork.People.GetByIdTrackingAsync(request.Id, cancellationToken);

            if (person == null)
            {
                _logger.LogError("Delete person command failed: person not found");

                throw new NotFoundException("Person not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            _unitOfWork.People.Delete(person, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
