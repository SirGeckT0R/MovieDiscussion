﻿using AutoMapper;
using DiscussionServiceApplication.RabbitMQ.Dto;
using DiscussionServiceApplication.RabbitMQ.Service;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Exceptions;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace DiscussionServiceApplication.Jobs
{
    public class DiscussionActivationJob(IUnitOfWork unitOfWork, 
                                         IRabbitMQService rabbitService,
                                         IMapper mapper,
                                         ILogger<DiscussionActivationJob> logger)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IRabbitMQService _rabbitService = rabbitService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<DiscussionActivationJob> _logger = logger;

        public async Task ExecuteAsync(Guid discussionId)
        {
            _logger.LogInformation("Background job to activate discussion with id {DiscussionId} started", discussionId);

            var discussion = await _unitOfWork.Discussions.GetByIdTrackingAsync(discussionId, default);

            if (discussion == null) 
            {
                _logger.LogError("Background job to activate discussion with id {DiscussionId} failed: discussion not found", discussionId);

                throw new NotFoundException("Discussion not found");
            }

            if (discussion.StartAt <= DateTime.UtcNow)
            {
                discussion.IsActive = true;
            }

            _unitOfWork.Discussions.Update(discussion, default);

            await _unitOfWork.SaveChangesAsync(default);

            if (discussion.Subscribers.Count > 0)
            {
                _logger.LogInformation("Notifying the subscribers that the discussion with id {Id} was activated", discussionId);

                var messageDto = _mapper.Map<DiscussionActivationDto>(discussion);

                await _rabbitService.SendSubscriptionMessage(messageDto, default);
            }

            _logger.LogInformation("Background job to activate discussion with id {DiscussionId} completed successfuly", discussionId);

            RecurringJob.RemoveIfExists(discussionId.ToString());
        }
    }
}
