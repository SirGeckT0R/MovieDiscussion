using AutoMapper;
using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.Movies.Commands.AddMovieCommand
{
    public class AddMovieHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICommandHandler<AddMovieCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(AddMovieCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!_unitOfWork.Genres.DoExist(request.Genres, cancellationToken))
            {
                throw new NotFoundException("Some genres are not found");
            }
            
            cancellationToken.ThrowIfCancellationRequested();
            if(!_unitOfWork.People.DoExist(request.CrewMembers.Select(x => x.PersonId).ToList(), cancellationToken))
            {
                throw new NotFoundException("Some crew members are not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var movie = _mapper.Map<Movie>(request);
            await _unitOfWork.Movies.AddAsync(movie, cancellationToken);

            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
