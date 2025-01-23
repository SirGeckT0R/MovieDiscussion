using AutoMapper;
using MediatR;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDataAccess.Specifications.ReviewSpecifications;
using MovieServiceDataAccess.Specifications.UserProfileSpecifications;
using MovieServiceDomain.Exceptions;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.Reviews.Commands.AddReviewCommand
{
    public class AddReviewHandler(IUnitOfWork unitOfWork, IMapper mapper) : ICommandHandler<AddReviewCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var candidateProfile = (await _unitOfWork.UserProfiles.GetWithSpecificationAsync(new UserProfileByAccountIdSpecification(request.AccountId), cancellationToken)).SingleOrDefault() 
                                    ?? throw new NotFoundException("User profile not found");
            _ = await _unitOfWork.Movies.GetByIdAsync(request.MovieId, cancellationToken) 
                ?? throw new NotFoundException("Movie not found");

            cancellationToken.ThrowIfCancellationRequested();
            var candidate = (await _unitOfWork.Reviews.GetWithSpecificationAsync(new ReviewByMovieAndProfileIdSpecification(candidateProfile.Id, request.MovieId), cancellationToken)).FirstOrDefault();
            if(candidate != null)
            {
                throw new ConflictException("Review by that user for the movie already exists");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var review = _mapper.Map<Review>(request);
            review.ProfileId = candidateProfile.Id;
            await _unitOfWork.Reviews.AddAsync(review, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
