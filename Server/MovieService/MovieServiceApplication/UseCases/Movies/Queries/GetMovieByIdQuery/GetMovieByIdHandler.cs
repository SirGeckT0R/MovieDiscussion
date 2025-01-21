using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Movies.Queries.GetMovieByIdQuery
{
    public class GetMovieByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetMovieByIdQuery, MovieDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<MovieDto> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Movie not found");

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<MovieDto>(movie);
        }
    }
}
