using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;
using MovieServiceDomain.Models;
namespace MovieServiceApplication.UseCases.Genres.Commands.UpdateGenreCommand
{
    public class UpdateGenreHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateGenreHandler> logger) : ICommandHandler<UpdateGenreCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdateGenreHandler> _logger = logger;

        public async Task<Unit> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            var candidateGenre = await _unitOfWork.Genres.GetByIdAsync(request.Id, cancellationToken);

            if (candidateGenre == null)
            {
                _logger.LogError("Update genre command failed for {Id}: genre not found", request.Id);

                throw new NotFoundException("Genre not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var genre = _mapper.Map<Genre>(request);
            _unitOfWork.Genres.Update(genre, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
