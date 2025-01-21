using AutoMapper;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.People.Commands.UpdatePersonCommand
{
    public class UpdatePersonMappingProfile : Profile
    {
        public UpdatePersonMappingProfile() {
            CreateMap<UpdatePersonCommand, Person>();
        }
    }
}
