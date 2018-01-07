using AutoMapper;
using BSRBanking.Model;
using BSRBanking.Utils;
using BSRBankingDataContract.Enums;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace BSRBanking.ViewModel
{
    public class BankAccountViewModel: ViewModelBase
    {
        private UserModel _userModel;
        private BankAccountModel _accountModel;

        private IFrameNavigationService _navigationService;

        public ICommand InternalTransferCommand { get; set; } 

        public string OwnerName
        {
            get
            {
                return _userModel.FirstName + " " + _userModel.LastNAme;
            }
        }

        public double Balance
        {
            get
            {
                return (double)_accountModel.Balance / 100;
            }
        }

        public string BankIdNumber
        {
            get
            {
                return _accountModel.BankAccountNumber;
            }
        }

        public BankAccountViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            _userModel = _navigationService.Parameter as UserModel;
            InternalTransferCommand = new RelayCommand(callInternalTransfer);
            getAccount(_userModel.UserId);
           
        }

        private void callInternalTransfer()
        {
            _navigationService.NavigateTo("InternalTransferPage",new { User = _userModel, Account = _accountModel});
        }

        private void getAccount(int userId)
        {
            using (var client = new Service.AccountManagerClient())
            {
                var result = client.GetBankAccount(userId);
                if (result.Result.Status == eOperationStatus.Success)
                {
                    _accountModel = Mapper.Map<BankAccountModel>(result.Data);
                }
            }
        }
    }
}
