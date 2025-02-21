using AutoMapper;
using UserServiceDataAccess.Models;

namespace UserGrpcServer.MappingProfiles
{
    public class GetUserInfoReplyMappingProfile : Profile
    {
        public GetUserInfoReplyMappingProfile()
        {
            CreateMap<User, GetUserInfoReply>();
        }
    }
}
