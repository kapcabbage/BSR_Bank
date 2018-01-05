using BSRBankingDataContract.Base;
using System.ServiceModel;


namespace BSRBankingService.Contracts
{
    [ServiceContract]
    public interface IAuthorization
    {
        [OperationContract]
        UserResultDto AuthenticateUser(string userName, string passwordHash); 
    }
}
