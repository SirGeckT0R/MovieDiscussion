using AutoMapper;
using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.People.Commands.AddPersonCommand
{
    public class AddPersonHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICommandHandler<AddPersonCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<Unit> Handle(AddPersonCommand request, CancellationToken cancellationToken)
        {
            var person = _mapper.Map<Person>(request);
            await _unitOfWork.People.AddAsync(person, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
