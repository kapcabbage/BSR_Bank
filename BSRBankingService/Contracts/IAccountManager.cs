using BSRBankingDataContract.Base;
using System.ServiceModel;

namespace BSRBankingService.Contracts
{
    [ServiceContract]
    public interface IAccountManager
    {
        [OperationContract]
        AccountActionListDto GetHistory(int bankAccountId);

        [OperationContract]
        BankResultDto GetBankAccount(int userId);


    }
}
