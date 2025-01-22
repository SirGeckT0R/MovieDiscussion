using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.MappingProfiles
{
    public class PersonDtoMappingProfile : Profile
    {
        public PersonDtoMappingProfile() 
        {
            CreateMap<Person, PersonDto>().ReverseMap();
        }
    }
}
