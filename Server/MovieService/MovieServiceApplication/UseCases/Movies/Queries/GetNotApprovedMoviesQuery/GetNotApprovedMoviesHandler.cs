using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceApplication.Interfaces.UseCases.Helpers;
using MovieServiceDataAccess.Interfaces.UnitOfWork;

namespace MovieServiceApplication.UseCases.Movies.Queries.GetNotApprovedMoviesQuery
{
    public class GetNotApprovedMoviesHandler(IUnitOfWork unitOfWork,
                                             IMapper mapper,
                                             IDetailedMovieHelper detailedMovieHelper)
                                             : IQueryHandler<GetNotApprovedMoviesQuery, ICollection<DetailedMovieDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IDetailedMovieHelper _detailedMovieHelper = detailedMovieHelper;

        public async Task<ICollection<DetailedMovieDto>> Handle(GetNotApprovedMoviesQuery request, 
                                                                CancellationToken cancellationToken)
        {
            var movies = await _unitOfWork.Movies.GetAllNotApprovedMoviesAsync(cancellationToken);
            var movieDtos = _mapper.Map<ICollection<DetailedMovieDto>>(movies);

            cancellationToken.ThrowIfCancellationRequested();
            var people = await _detailedMovieHelper.GetDetailedCrewAsync(movies, cancellationToken);
            _detailedMovieHelper.SetDetailedCrew(movieDtos, people);

            cancellationToken.ThrowIfCancellationRequested();
            var genres = await _detailedMovieHelper.GetDetailedGenresAsync(movies, cancellationToken);
            _detailedMovieHelper.SetDetailedGenres(movieDtos, genres);

            return movieDtos;
        }
    }
}

