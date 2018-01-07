using AutoMapper;
using BSRBanking.Model;
using BSRBanking.Utils;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BSRBanking.ViewModel
{

    public class LoginViewModel: ViewModelBase
    {
        private string _userName;

        public ICommand loginCommand { get; set; }
 

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
            loginCommand = new RelayCommand<object>(Login);
        }

        public void Login(object parameter)
        {
            using(var client = new Service.AuthorizationClient())
            {
                var passwordBox = parameter as PasswordBox;
                var passwordHash = Computing.Sha256(passwordBox.Password); 
                var result = client.AuthenticateUser(this._userName, passwordHash);
                if(result.Result.Status == Service.eOperationStatus.Success)
                {
                    var userModel = Mapper.Map<UserModel>(result.Data);
                    _navigationService.NavigateTo("BankPage", userModel);
                }
            }
        }
    }
}
