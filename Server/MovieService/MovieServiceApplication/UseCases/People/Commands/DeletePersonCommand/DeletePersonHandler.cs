using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.People.Commands.DeletePersonCommand
{
    public class DeletePersonHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeletePersonCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            var person = await _unitOfWork.People.GetByIdTrackingAsync(request.Id, cancellationToken);

            if (person == null)
            {
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
