using BSRBankingDataContract.Base;
using BSRBankingDataContract.Dtos;
using BSRBankingService.Contracts;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;
using Dapper;
using System.Linq;

namespace BSRBankingService.Services
{
    public partial class Service : IAccountManager
    {
        public BankResultDto GetBankAccount(int userId)
        {
            var result = new BankResultDto();
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConnString"].ConnectionString))
                {
                    var query = "SELECT TOP 1 * FROM BankAccounts where OwnerId = @userid";
                    var any = connection.Query<BankAccountDto>(query, new { userid = userId}).AsList();
                    if (any.Count == 0)
                    {
                        result.SetStatus(BSRBankingDataContract.Enums.eOperationStatus.GeneralError);
                    }
                    else
                    {
                        result.SetSuccess(any.FirstOrDefault());
                    }

                }
            }
            catch (Exception ex)
            {
                result.SetErrors(ex);
            }

            return result;
        }

        public AccountActionListDto GetHistory(int bankAccountId)
        {
            throw new NotImplementedException();
        }

        public BoolResultDto Transfer(AccountActionDto accountAction)
        {
            var result = new BoolResultDto();
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConnString"].ConnectionString))
                {
                   using(var transactionScope = new TransactionScope())
                   {
                        var querySelf = "update BankAccounts set Balance = (Balance - @amount) OUTPUT INSERTED.BankAccountId where BankAccountNumber = @bankId";
                        var queryDest = "update BankAccounts set Balance = (Balance + @amount) OUTPUT INSERTED.BankAccountId where BankAccountNumber = @bankId";
                        var insertAction= "insert into AccountActions OUTPUT INSERTED.AccountActionId values(@actionType, @title, @sourceName, @destName, @sourceNumber, @destNumber, @amount)";
                        var insertHistory = "insert into BankAccountHistory OUTPUT INSERTED.BankAccountHistoryId values(@actionId, @bankId, (select Balance from BankAccounts where BankAccountNumber = @bankNumber), @date)";
                        var updateSelfResult = connection.Query<int>(querySelf, new { bankId = accountAction.SourceBankNumber, amount = accountAction.Amount}).AsList();
                        var updateDestResult = connection.Query<int>(queryDest, new { bankId = accountAction.DestinationBankNumber, amount = accountAction.Amount }).AsList(); ;
                        var insertActionResult = connection.Query<int>(insertAction, new
                        {
                            actionType = (int)accountAction.ActionType,
                            title = accountAction.Title,
                            sourceName = accountAction.SourceName,
                            destName = accountAction.DestinationName,
                            sourceNumber = accountAction.SourceBankNumber,
                            destNumber = accountAction.DestinationBankNumber,
                            amount = accountAction.Amount
                        }).AsList();
                        var insertSelfHistory = connection.Query<int>(insertHistory, new { actionId = insertActionResult.First(), bankId = updateSelfResult.First(), bankNumber = accountAction.SourceBankNumber,date = DateTime.Now});
                        var insertDestHistory = connection.Query<int>(insertHistory, new { actionId = insertActionResult.First(), bankId = updateDestResult.First(), bankNumber = accountAction.DestinationBankNumber ,date = DateTime.Now });
                        if(insertDestHistory.First() != 0 && insertSelfHistory.First() != 0)
                        {
                            transactionScope.Complete();
                            result.SetSuccess(true);
                        }

                    }

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
