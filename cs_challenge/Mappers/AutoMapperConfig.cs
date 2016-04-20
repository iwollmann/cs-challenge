using AutoMapper;
using cs_challenge.Contracts;
using cs_challenge.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cs_challenge.Mappers
{
    public class AutoMapperConfig
    {
        public static IMapper RegisterMappings() {
            var config = new MapperConfiguration((cfg) => {
                cfg.CreateMap<CreateUserContract, User>();
                cfg.CreateMap<User, UserContract>();
            });

            return config.CreateMapper();
        }
    }
}