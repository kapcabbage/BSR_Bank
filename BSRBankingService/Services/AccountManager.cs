using BSRBankingDataContract.Base;
using BSRBankingDataContract.Dtos;
using BSRBankingService.Contracts;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;
using Dapper;
using System.Linq;
using BSRBankingDataAccess;

namespace BSRBankingService.Services
{
    public partial class Service : IAccountManager
    {
        public BankResultDto GetBankAccount(int userId)
        {
            var result = new BankResultDto();
            try
            {
               var bankResult = BankAccount.GetBankAccount(userId);
                if(bankResult == null)
                {
                    result.SetErrors("No bank account found");
                }
                else
                {
                    result.SetSuccess(bankResult);
                }
            }
            catch (Exception ex)
            {
                result.SetErrors(ex);
            }

            return result;
        }

        public HistoryListResultDto GetHistory(int bankAccountId)
        {
            var result = new HistoryListResultDto();
            try
            {
                var historyResult = BankAccount.GetHistory(bankAccountId);
                if (historyResult != null)
                {
                    result.SetSuccess(historyResult);
                }
            }
            catch (Exception ex)
            {
                result.SetErrors(ex);
            }
            return result;
        }

        public BoolResultDto Transfer(AccountActionDto accountAction)
        {
            var result = new BoolResultDto();
            try
            {
                var transferResult = BankAccount.InternalTransfer(accountAction);
                if (transferResult)
                {
                    result.SetSuccess(transferResult);
                }
            }
            catch (Exception ex)
            {
                result.SetErrors(ex);
            }
            return result;
        }
    }
}
