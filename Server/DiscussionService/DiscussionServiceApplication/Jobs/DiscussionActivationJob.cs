using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Exceptions;
using Hangfire;

namespace DiscussionServiceApplication.Jobs
{
    public class DiscussionActivationJob(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task ExecuteAsync(Guid discussionId)
        {
            var discussion = await _unitOfWork.Discussions.GetByIdTrackingAsync(discussionId, default);

            if (discussion == null) {
                throw new NotFoundException("Discussion not found");
            }

            if (discussion.StartAt <= DateTime.UtcNow)
            {
                discussion.IsActive = true;
            }

            _unitOfWork.Discussions.Update(discussion, default);

            await _unitOfWork.SaveChangesAsync(default);

            RecurringJob.RemoveIfExists(discussionId.ToString());
        }
    }
}
