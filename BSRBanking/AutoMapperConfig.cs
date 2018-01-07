using AutoMapper;
using BSRBanking.Model;
using BSRBankingDataContract.Dtos;
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
                cfg.CreateMap<BankAccountModel, BankAccountDto>();
                cfg.CreateMap<BankAccountDto, BankAccountModel>();
                cfg.CreateMap<AccountActionDto, AccountActionModel>();
                cfg.CreateMap<AccountActionModel, AccountActionDto>();
            });
        }
    }
    
}
