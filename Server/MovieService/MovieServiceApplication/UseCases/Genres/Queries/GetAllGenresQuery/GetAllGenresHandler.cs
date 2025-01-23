using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;

namespace MovieServiceApplication.UseCases.Genres.Queries.GetAllGenresQuery
{
    public class GetAllGenresHandler(IUnitOfWork unitOfWork, IMapper mapper) : IQueryHandler<GetAllGenresQuery, ICollection<GenreDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<ICollection<GenreDto>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
        {
            var genres = await _unitOfWork.Genres.GetAllAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<ICollection<GenreDto>>(genres);
        }
    }
}
