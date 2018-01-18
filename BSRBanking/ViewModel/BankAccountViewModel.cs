using AutoMapper;
using BSRBanking.Model;
using BSRBanking.Utils;
using BSRBankingDataContract.Dtos;
using BSRBankingDataContract.Enums;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BSRBanking.ViewModel
{
    public class BankAccountViewModel: ViewModelBase
    {
        private UserModel _userModel;
        private BankAccountModel _accountModel;
        private int _amount;
        private string _errorMessage;
        private int _balance;
        private Visibility _errorLabel;

        private IFrameNavigationService _navigationService;
        
        public ICommand TransferCommand { get; set; }
        public ICommand Refresh { get; set; }
        public ICommand Withdraw { get; set; }
        public ICommand Deposit { get; set; }

        public ObservableCollection<HistoryEntryModel> History { get; set; }

        public Visibility ErrorLabel
        {
            get
            {
                return _errorLabel;
            }
            set
            {
                _errorLabel = value;

                RaisePropertyChanged("ErrorLabel");
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;

                RaisePropertyChanged("ErrorMessage");
            }
        }

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
            set
            {
                _balance = (int)value;
                RaisePropertyChanged("Balance");
            }

        }

        public string BankIdNumber
        {
            get
            {
                return _accountModel.BankAccountNumber;
            }
        }

        public int Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                try
                {
                    _amount = value;
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                    ErrorLabel = Visibility.Visible;
                }
            }
        }

        public BankAccountViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            _userModel = _navigationService.Parameter as UserModel;
            TransferCommand = new RelayCommand(callTransfer);
            Refresh = new RelayCommand(refresh);
            Deposit = new RelayCommand(deposit);
            Withdraw = new RelayCommand(withdrawal);
            History = new ObservableCollection<HistoryEntryModel>();
            getAccount(_userModel.UserId);
            getHistory(_accountModel.BankAccountId);
            _errorLabel = Visibility.Hidden;

        }

        private void callTransfer()
        {
            _navigationService.NavigateTo("TransferPage",new { User = _userModel, Account = _accountModel});
        }

        private void refresh()
        {
            getAccount(_userModel.UserId);
            Balance = _accountModel.Balance;
            getHistory(_accountModel.BankAccountId);
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
            History.Clear();
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

        private void deposit()
        {
            try
            {
                var action = new AccountActionModel(eActionType.Deposit, OwnerName, BankIdNumber, OwnerName, BankIdNumber, "Deposit", Amount);
                var actionDto = Mapper.Map<AccountActionDto>(action);
                ErrorLabel = Visibility.Hidden;
                using (var client = new Service.AccountManagerClient())
                {
                    var result = client.Deposit(actionDto);
                    if (result.Success())
                    {
                        refresh();
                        //_navigationService.NavigateTo("BankPage", "");
                    }
                    else
                    {
                        ErrorMessage = result.Result.ExceptionMessage;
                        ErrorLabel = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                ErrorLabel = Visibility.Visible;
            }
        }

        private void withdrawal()
        {
            try
            {
                var action = new AccountActionModel(eActionType.Withdrawal, OwnerName, BankIdNumber, OwnerName, BankIdNumber, "Deposit", Amount);
                var actionDto = Mapper.Map<AccountActionDto>(action);
                ErrorLabel = Visibility.Hidden;
                using (var client = new Service.AccountManagerClient())
                {
                    var result = client.Withdraw(actionDto);
                    if (result.Success())
                    {
                        refresh();
                        //_navigationService.NavigateTo("BankPage", "");
                    }
                    else
                    {
                        ErrorMessage = result.Result.ExceptionMessage;
                        ErrorLabel = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                ErrorLabel = Visibility.Visible;
            }
        }
    }
}
