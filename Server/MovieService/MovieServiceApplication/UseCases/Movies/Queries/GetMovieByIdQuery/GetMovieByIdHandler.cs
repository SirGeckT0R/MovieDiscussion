using AutoMapper;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceApplication.Interfaces.UseCases.Helpers;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Movies.Queries.GetMovieByIdQuery
{
    public class GetMovieByIdHandler(IUnitOfWork unitOfWork, 
                                     IMapper mapper,
                                     IDetailedMovieHelper detailedMovieHelper,
                                     ILogger<GetMovieByIdHandler> logger) : IQueryHandler<GetMovieByIdQuery, DetailedMovieDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IDetailedMovieHelper _detailedMovieHelper = detailedMovieHelper;
        private readonly ILogger<GetMovieByIdHandler> _logger = logger;

        public async Task<DetailedMovieDto> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(request.Id, cancellationToken);

            if (movie == null)
            {
                _logger.LogError("Get movie by id {Id} query failed: movie not found", request.Id);

                throw new NotFoundException("Movie not found");
            }

            var movieDto = _mapper.Map<DetailedMovieDto>(movie);

            cancellationToken.ThrowIfCancellationRequested();
            var people = await _detailedMovieHelper.GetDetailedCrewAsync(movie, cancellationToken);
            _detailedMovieHelper.SetDetailedCrew(movieDto, people);

            cancellationToken.ThrowIfCancellationRequested();
            var genres = await _detailedMovieHelper.GetDetailedGenresAsync(movie,  cancellationToken);
            _detailedMovieHelper.SetDetailedGenres(movieDto, genres);

            return movieDto;
        }
    }
}
