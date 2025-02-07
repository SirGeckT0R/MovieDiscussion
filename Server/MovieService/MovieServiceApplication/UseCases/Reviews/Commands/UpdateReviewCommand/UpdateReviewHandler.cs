using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MovieServiceApplication.Helpers;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Exceptions;

namespace MovieServiceApplication.UseCases.Reviews.Commands.UpdateReviewCommand
{
    public class UpdateReviewHandler(IUnitOfWork unitOfWork, 
                                     IMapper mapper, 
                                     ILogger<UpdateReviewHandler> logger,
                                     IConfiguration configuration) 
                                     : 
                                     ICommandHandler<UpdateReviewCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdateReviewHandler> _logger = logger;
        private readonly IConfiguration _configuration = configuration;

        public async Task<Unit> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.Reviews.GetByIdTrackingAsync(request.Id, cancellationToken);

            if (review == null)
            {
                _logger.LogError("Update review command failed for {Id}: review not found", request.Id);

                throw new NotFoundException("Review not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var newReview = _mapper.Map(request, review);
            _unitOfWork.Reviews.Update(newReview, cancellationToken);

            CalculateRatingJobHelper.AddJob(review.MovieId, _configuration, _logger);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
