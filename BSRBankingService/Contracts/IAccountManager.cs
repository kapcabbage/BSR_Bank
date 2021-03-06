﻿using BSRBankingDataContract.Base;
using BSRBankingDataContract.Dtos;
using System.ServiceModel;

namespace BSRBankingService.Contracts
{
    [ServiceContract]
    public interface IAccountManager
    {
        [OperationContract]
        HistoryListResultDto GetHistory(int bankAccountId);

        [OperationContract]
        BankResultDto GetBankAccount(int userId);

        [OperationContract]
        BoolResultDto Transfer(AccountActionDto action);

        [OperationContract]
        BoolResultDto External(AccountActionDto action);

        [OperationContract]
        BoolResultDto Withdraw(AccountActionDto action);

        [OperationContract]
        BoolResultDto Deposit(AccountActionDto action);

    }
}
