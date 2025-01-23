using AutoMapper;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.UserProfiles.Commands.UpdateUserProfileCommand
{
    public class UpdateUserProfileMappingProfile : Profile
    {
        public UpdateUserProfileMappingProfile() 
        {
            CreateMap<UpdateUserProfileCommand, UserProfile>();
        }
    }
}
