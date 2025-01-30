using DiscussionServiceDomain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace DiscussionServiceDataAccess.Configurations
{
    public class DiscussionConfiguration : IEntityTypeConfiguration<Discussion>
    {
        public void Configure(EntityTypeBuilder<Discussion> builder)
        {
            builder
                .ToCollection("discussions")
                .OwnsMany
                (
                    discussion => discussion.Messages,

                    message =>
                    {
                        message.WithOwner().HasForeignKey("OwnerId");
                        message.Property<Guid>("Id");
                        message.HasKey("Id");
                    }
                );
        }
    }
    }
