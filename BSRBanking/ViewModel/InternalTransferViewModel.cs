using AutoMapper;
using BSRBanking.Model;
using BSRBanking.Utils;
using BSRBankingDataContract.Dtos;
using BSRBankingDataContract.Enums;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BSRBanking.ViewModel
{
    public class InternalTransferViewModel : ViewModelBase
    {
        private UserModel _userModel;
        private BankAccountModel _accountModel;
        private string _destinationName;
        private string _destinationNumber;
        private int _amount;
        private string _title;
        private string _errorMessage;


        private Visibility _errorLabel;

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
        private IFrameNavigationService _navigationService;
        public ICommand IssueTransfer { get; set; }
                public ICommand CancelCommand { get; set; }

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
        }

        public string BankIdNumber
        {
            get
            {
                return _accountModel.BankAccountNumber;
            }
        }

        public string DestinationOwnerName
        {
            get
            {
                return _destinationName;
            }
            set
            {
                _destinationName = value;
            }
        }

        public string DestinationIdNumber
        {
            get
            {
                return _destinationNumber ; 
            }
            set
            {
                _destinationNumber = value;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
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
                }catch(Exception ex)
                {
                    ErrorMessage = ex.Message;
                    ErrorLabel = Visibility.Visible;
                }
            }
        }

        public InternalTransferViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            IssueTransfer = new RelayCommand(InternalTransfer);
            CancelCommand = new RelayCommand(CancelTransfer);
            dynamic parameters = _navigationService.Parameter;
            _userModel = parameters.User;
            _accountModel = parameters.Account;
            _errorLabel = Visibility.Hidden;
           
        }

        private void InternalTransfer()
        {
            var s = Computing.ComputeChecksum("001168340011225033158321");
            try
            {
                var action = new AccountActionModel(eActionType.InternalTransfer, DestinationOwnerName, DestinationIdNumber, OwnerName, BankIdNumber, Title, Amount);
                var actionDto = Mapper.Map<AccountActionDto>(action);
                ErrorLabel = Visibility.Hidden;
                using (var client = new Service.AccountManagerClient())
                {
                    var result = client.Transfer(actionDto);
                    if (result.Success())
                    {
                        _navigationService.NavigateTo("BankPage", "");
                    }
                    else
                    {
                        ErrorMessage = result.Result.ExceptionMessage;
                        ErrorLabel = Visibility.Visible;
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                ErrorLabel = Visibility.Visible;
            }
        }

        private void CancelTransfer()
        {
            _navigationService.NavigateTo("BankPage", "");
        }

    }
}
