using AutoMapper;
using BSRBanking.Model;
using BSRBanking.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSRBanking
{
    public static class AutoMapperConfig
    {
        public static void Init()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<UserModel, UserDto>();
                cfg.CreateMap<UserDto, UserModel>();
            });
        }
    }
    
}
