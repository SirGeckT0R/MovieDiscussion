using AutoMapper;
using DiscussionServiceApplication.Dto;
using DiscussionServiceApplication.Interfaces.UseCases;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;
using DiscussionServiceDomain.Exceptions;
using Microsoft.Extensions.Logging;

namespace DiscussionServiceApplication.UseCases.Messages.Queries.GetMessageByIdQuery
{
    public class GetMessageByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetMessageByIdHandler> logger) : IQueryHandler<GetMessageByIdQuery, MessageDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetMessageByIdHandler> _logger = logger;

        public async Task<MessageDto> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
        {
            var message = await _unitOfWork.Messages.GetByIdAsync(request.Id, cancellationToken);

            if (message == null)
            {
                _logger.LogError("Get message by id {Id} query failed: message not found", request.Id);

                throw new NotFoundException("Message not found");
            }

            cancellationToken.ThrowIfCancellationRequested();

            return _mapper.Map<MessageDto>(message);
        }
    }
}
