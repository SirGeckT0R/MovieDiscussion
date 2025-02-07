using AutoMapper;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Genres.Queries.GetGenreByIdQuery
{
    public class GetGenreByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetGenreByIdHandler> logger) : IQueryHandler<GetGenreByIdQuery, GenreDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetGenreByIdHandler> _logger = logger;

        public async Task<GenreDto> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            var genre = await _unitOfWork.Genres.GetByIdAsync(request.Id, cancellationToken);

            if (genre == null)
            {
                _logger.LogError("Get genre by id {Id} query failed: genre not found", request.Id);

                throw new NotFoundException("Genre not found");
            }

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<GenreDto>(genre);
        }
    }
}
