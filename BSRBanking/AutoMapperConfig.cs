using AutoMapper;
using BSRBanking.Model;
using BSRBankingDataContract.Dtos;


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
                cfg.CreateMap<HistoryEntryDto, HistoryEntryModel>()
                .ForMember(dest=>dest.Amount,opts=>opts.MapFrom(src=>(double)src.Amount/100))
                .ForMember(dest=>dest.BalanceAfter, opts=>opts.MapFrom(src=>(double)src.BalanceAfter/100));
                cfg.CreateMap<HistoryEntryModel, HistoryEntryDto>();
            });
        }
    }
    
}
