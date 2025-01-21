using AutoMapper;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.People.Commands.AddPersonCommand
{
    public class AddPersonMappingProfile : Profile
    {
        public AddPersonMappingProfile() {
            CreateMap<AddPersonCommand, Person>();
        }
    }
}
