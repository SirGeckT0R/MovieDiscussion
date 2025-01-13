﻿using AutoMapper;
using UserServiceApplication.Dto;
using UserServiceDataAccess.Enums;
using UserServiceDataAccess.Models;

namespace UserServiceApplication.Mapper
{
    public class RegisterRequestMappingProfile : Profile
    {
        public RegisterRequestMappingProfile()
        {
            CreateMap<RegisterRequest, User>()
                .AfterMap((com, user) => user.Role = E_Role.User)
                ;
        }
    }
}
