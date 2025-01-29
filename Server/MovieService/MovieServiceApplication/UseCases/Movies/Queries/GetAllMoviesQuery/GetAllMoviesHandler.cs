using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;

namespace MovieServiceApplication.UseCases.Movies.Queries.GetAllMoviesQuery
{
    public class GetAllMoviesHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetAllMoviesQuery, ICollection<MovieDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ICollection<MovieDto>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _unitOfWork.Movies.GetAllAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<ICollection<MovieDto>>(movies);
        }
    }
}
