using BSRBanking.Model;
using BSRBanking.Utils;
using GalaSoft.MvvmLight;


namespace BSRBanking.ViewModel
{
    public class BankAccountViewModel: ViewModelBase
    {
        private UserModel _userModel;
        private BankAccountModel _accountModel;

        private IFrameNavigationService _navigationService;
        
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
        }
    }
}
