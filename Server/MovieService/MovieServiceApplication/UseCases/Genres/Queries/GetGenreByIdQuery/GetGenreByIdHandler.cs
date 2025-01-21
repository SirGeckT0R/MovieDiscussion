using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Genres.Queries.GetGenreByIdQuery
{
    public class GetGenreByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetGenreByIdQuery, GenreDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<GenreDto> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            var genre = await _unitOfWork.Genres.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Genre not found");

            cancellationToken.ThrowIfCancellationRequested();
            
            return _mapper.Map<GenreDto>(genre);
        }
    }
}
