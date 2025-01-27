using AutoMapper;
using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.People.Commands.UpdatePersonCommand
{
    public class UpdatePersonHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICommandHandler<UpdatePersonCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<Unit> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            var candidatePerson = await _unitOfWork.People.GetByIdAsync(request.Id, cancellationToken);

            if (candidatePerson == null)
            {
                throw new NotFoundException("Person not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var person = _mapper.Map<Person>(request);
            _unitOfWork.People.Update(person, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
