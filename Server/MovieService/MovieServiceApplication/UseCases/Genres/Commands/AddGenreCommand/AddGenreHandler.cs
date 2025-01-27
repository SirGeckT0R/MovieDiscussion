using AutoMapper;
using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.GenreSpecifications;
using MovieServiceDomain.Exceptions;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.Genres.Commands.AddGenreCommand
{
    public class AddGenreHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICommandHandler<AddGenreCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<Unit> Handle(AddGenreCommand request, CancellationToken cancellationToken)
        {
            var genreSpecification = new GenreByNameSpecification(request.Name);
            var candidates = await _unitOfWork.Genres.GetWithSpecificationAsync(genreSpecification, cancellationToken);
            var candidateGenre = candidates.SingleOrDefault();

            if (candidateGenre != null)
            {
                throw new ConflictException("Genre already exists");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var genre = _mapper.Map<Genre>(request);
            await _unitOfWork.Genres.AddAsync(genre, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
