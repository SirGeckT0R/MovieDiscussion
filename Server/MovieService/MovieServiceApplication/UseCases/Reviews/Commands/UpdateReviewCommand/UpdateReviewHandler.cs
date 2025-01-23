using AutoMapper;
using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Reviews.Commands.UpdateReviewCommand
{
    public class UpdateReviewHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICommandHandler<UpdateReviewCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.Reviews.GetByIdTrackingAsync(request.Id, cancellationToken);
            if (review == null)
            {
                throw new NotFoundException("Review not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var newReview = _mapper.Map(request, review);
            _unitOfWork.Reviews.Update(newReview, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
