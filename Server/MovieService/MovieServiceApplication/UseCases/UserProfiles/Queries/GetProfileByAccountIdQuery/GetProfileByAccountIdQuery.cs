using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.UserProfiles.Queries.GetProfileByAccountIdQuery
{
    public record GetProfileByAccountIdQuery(Guid AccountId) : IQuery<UserProfileDto>;
}
