using AutoMapper;
using BSRBanking.Model;
using BSRBanking.Utils;
using BSRBankingDataContract.Enums;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BSRBanking.ViewModel
{

    public class LoginViewModel: ViewModelBase
    {
        private string _userName;
        private Visibility _errorLabel;

        public ICommand LoginCommand { get; set; }
 
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

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        private IFrameNavigationService _navigationService;

        public LoginViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            LoginCommand = new RelayCommand<object>(Login);
            ErrorLabel = Visibility.Hidden;
        }

        public void Login(object parameter)
        {
            using(var client = new Service.AuthorizationClient())
            {
                var passwordBox = parameter as PasswordBox;
                var passwordHash = Computing.Sha256(passwordBox.Password); 
                var result = client.AuthenticateUser(this._userName, passwordHash);
                if(result.Result.Status == eOperationStatus.Success)
                {
                    var userModel = Mapper.Map<UserModel>(result.Data);
                    _navigationService.NavigateTo("BankPage", userModel);
                }
                else
                {
                    ErrorLabel = Visibility.Visible;
                }
            }
        }
    }
}
