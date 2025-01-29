using AutoMapper;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.UserProfiles.Commands.CreateUserProfileCommand
{
    public class CreateUserProfileMappingProfile : Profile
    {
        public CreateUserProfileMappingProfile()
        {
            CreateMap<CreateUserProfileCommand, UserProfile>();
        }
    }
}
