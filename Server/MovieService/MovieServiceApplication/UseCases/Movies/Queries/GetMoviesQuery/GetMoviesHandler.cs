using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.MovieSpecification;

namespace MovieServiceApplication.UseCases.Movies.Queries.GetAllMoviesQuery
{
    public class GetMoviesHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetMoviesQuery, PaginatedCollection<MovieDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<PaginatedCollection<MovieDto>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        {
            var specification = new MovieByNameSpecification(request.Name);
            var (movies, totalPages) = await _unitOfWork.Movies.GetPaginatedWithSpecificationAsync(
                                                                                     specification, 
                                                                                     request.PageIndex,
                                                                                     request.PageSize, 
                                                                                     cancellationToken
                                                                                     );

            cancellationToken.ThrowIfCancellationRequested();

            var movieDtoCollection = _mapper.Map<ICollection<MovieDto>>(movies);

            var paginatedCollection = new PaginatedCollection<MovieDto>(movieDtoCollection, request.PageIndex, totalPages);

            return paginatedCollection;
        }
    }
}
