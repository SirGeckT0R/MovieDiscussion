using AutoMapper;
using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Exceptions;
using DiscussionServiceDomain.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DiscussionServiceApplication.UseCases.Messages.Commands.AddMessageToDiscussionCommand
{
    public class AddMessageToDiscussionHandler(IUnitOfWork unitOfWork, 
                                               IDistributedCache cache, 
                                               ILogger<AddMessageToDiscussionHandler> logger,
                                               IMapper mapper) : ICommandHandler<AddMessageToDiscussionCommand, (UserConnection, MessageDto)>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDistributedCache _cache = cache;
        private readonly ILogger<AddMessageToDiscussionHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<(UserConnection, MessageDto)> Handle(AddMessageToDiscussionCommand request, CancellationToken cancellationToken)
        {
            var stringConnection = await _cache.GetStringAsync(request.ConnectionId, cancellationToken);

            if (string.IsNullOrWhiteSpace(stringConnection))
            {
                _logger.LogError("Add message command failed for {ConnectionId}: user connection not found in cache", request.ConnectionId);

                throw new NotFoundException("User connection not found in cache");
            }

            var userConnection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

            if (userConnection == null)
            {
                _logger.LogError("Add message command failed for {ConnectionId}: user connection can't be parsed", request.ConnectionId);

                throw new CacheException("User connection can't be parsed");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var discussion = await _unitOfWork.Discussions.GetByIdAsync(userConnection.DiscussionId, cancellationToken);

            if (discussion == null)
            {
                _logger.LogError("Add message command failed for {ConnectionId}: discussion with id {DiscussionId} not found", request.ConnectionId, userConnection.DiscussionId);

                throw new NotFoundException("Discussion not found");
            }

            cancellationToken.ThrowIfCancellationRequested();
            var message = new Message(userConnection.DiscussionId, request.Text, userConnection.AccountId, userConnection.Username);
            await _unitOfWork.Messages.AddAsync(message, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var messageDto = _mapper.Map<MessageDto>(message);

            return (userConnection, messageDto);
        }
    }
}
