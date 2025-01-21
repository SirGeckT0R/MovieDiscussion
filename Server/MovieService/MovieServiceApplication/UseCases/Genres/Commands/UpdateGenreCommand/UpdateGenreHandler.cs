using AutoMapper;
using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;
using MovieServiceDomain.Models;
namespace MovieServiceApplication.UseCases.Genres.Commands.UpdateGenreCommand
{
    public class UpdateGenreHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICommandHandler<UpdateGenreCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<Unit> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            _ = await _unitOfWork.Genres.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException("Genre not found");

            cancellationToken.ThrowIfCancellationRequested();
            var genre = _mapper.Map<Genre>(request);
            _unitOfWork.Genres.Update(genre, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
