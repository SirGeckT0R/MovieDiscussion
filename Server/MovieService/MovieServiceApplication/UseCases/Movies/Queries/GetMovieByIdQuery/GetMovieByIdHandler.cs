using AutoMapper;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Movies.Queries.GetMovieByIdQuery
{
    public class GetMovieByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetMovieByIdHandler> logger) : IQueryHandler<GetMovieByIdQuery, DetailedMovieDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetMovieByIdHandler> _logger = logger;

        public async Task<DetailedMovieDto> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(request.Id, cancellationToken);

            if (movie == null)
            {
                _logger.LogError("Get movie by id {Id} query failed: movie not found", request.Id);

                throw new NotFoundException("Movie not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var crewMemberIds = movie.CrewMembers.Select(x => x.PersonId);
            var people = await _unitOfWork.People.GetFromListOfIdsAsync(crewMemberIds, cancellationToken);

            var movieDto = _mapper.Map<DetailedMovieDto>(movie);
            var crewList = movieDto.CrewMembers.Join(people,
                                                 x => x.PersonId,
                                                 y => y.Id,
                                                 (x, y) => new CrewMemberDto { PersonId = x.PersonId, Role = x.Role, FullName = $"{y.FirstName} {y.LastName}" })
                                               .ToList();

            movieDto.CrewMembers = crewList;

            cancellationToken.ThrowIfCancellationRequested();
            var genres = await _unitOfWork.Genres.GetFromListOfIdsAsync(movie.Genres, cancellationToken);

            var genreList = movieDto.Genres.Join(genres,
                                                 x => x.Id,
                                                 y => y.Id,
                                                 (x, y) => new GenreDto { Id = x.Id, Name = y.Name })
                                           .ToList();

            movieDto.Genres = genreList;

            return movieDto;
        }
    }
}
