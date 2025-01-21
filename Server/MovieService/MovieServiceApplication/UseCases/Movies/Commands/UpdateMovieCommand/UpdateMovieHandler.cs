using AutoMapper;
using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.Movies.Commands.UpdateMovieCommand
{
    public class UpdateMovieHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICommandHandler<UpdateMovieCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _unitOfWork.Movies.GetByIdTrackingAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Movie not found");

            cancellationToken.ThrowIfCancellationRequested();
            if (!_unitOfWork.Genres.DoExist(request.Genres, cancellationToken))
            {
                throw new NotFoundException("Some genres are not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            if (!_unitOfWork.People.DoExist(request.CrewMembers.Select(x => x.PersonId).ToList(), cancellationToken))
            {
                throw new NotFoundException("Some crew members are not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var movie = _mapper.Map(request, candidate);
            _unitOfWork.Movies.Update(movie, cancellationToken);

            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
