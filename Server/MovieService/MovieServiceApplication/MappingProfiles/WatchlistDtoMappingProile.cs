using AutoMapper;
using MovieServiceApplication.Dto;
using MovieServiceDomain.Models;
namespace MovieServiceApplication.MappingProfiles
{
    public class WatchlistDtoMappingProile : Profile
    {
        public WatchlistDtoMappingProile() 
        {
            CreateMap<Watchlist, WatchlistDto>().ReverseMap();
        }
    }
}
