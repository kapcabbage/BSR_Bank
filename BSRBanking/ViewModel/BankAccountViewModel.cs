using AutoMapper;
using BSRBanking.Model;
using BSRBanking.Utils;
using BSRBankingDataContract.Enums;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BSRBanking.ViewModel
{
    public class BankAccountViewModel: ViewModelBase
    {
        private UserModel _userModel;
        private BankAccountModel _accountModel;

        private IFrameNavigationService _navigationService;

        public ICommand InternalTransferCommand { get; set; } 
        public ObservableCollection<HistoryEntryModel> History { get; set; }

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
            History = new ObservableCollection<HistoryEntryModel>();
            getAccount(_userModel.UserId);
            getHistory(_accountModel.BankAccountId);
           
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
                if (result.Success())
                {
                    _accountModel = Mapper.Map<BankAccountModel>(result.Data);
                }
            }
        }

        private void getHistory(int bankId)
        {
            var s = Computing.ComputeChecksum("001168340011225033158322");
            using(var client = new Service.AccountManagerClient())
            {
                var result = client.GetHistory(bankId);
                if (result.Success())
                {
                    var historyList = Mapper.Map<List<HistoryEntryModel>>(result.Data);
                    foreach(var entry in historyList)
                    {
                        History.Add(entry);
                    }
                }
            }
        }
    }
}
