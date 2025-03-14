﻿using DiscussionServiceDataAccess.DatabaseContext;
using DiscussionServiceDataAccess.Interfaces.Repositories;
using DiscussionServiceDataAccess.Interfaces.UnitOfWork;

namespace DiscussionServiceDataAccess.UnitOfWork
{
    public class DiscussionUnitOfWork(DiscussionDbContext context,
                                      IDiscussionRepository discussionRepository,
                                      IMessageRepository messageRepository) : IUnitOfWork
    {
        private readonly DiscussionDbContext _context = context;
        private readonly IDiscussionRepository _discussionRepository = discussionRepository;
        private readonly IMessageRepository _messageRepository = messageRepository;

        private bool disposed = false;
        public IDiscussionRepository Discussions
        {
            get
            {
                return _discussionRepository;
            }
        }

        public IMessageRepository Messages
        {
            get
            {
                return _messageRepository;
            }
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                _context.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
