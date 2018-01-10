using BSRBankingDataContract.Base;
using BSRBankingDataContract.Dtos;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Transactions;
using System.IO;

namespace BSRBankingDataAccess
{
    public static class BankAccount
    {
        public static List<HistoryEntryDto> GetHistory(int bankAccountId)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConnString"].ConnectionString))
            {
                var query = "select * from History_V where BankAccountId = @bankId";
                var history = connection.Query<HistoryEntryDto>(query, new { bankId = bankAccountId }).AsList();
                if (history != null)
                {
                    return history;
                }
                else return new List<HistoryEntryDto>();
            }
        }

        public static BankAccountDto GetBankAccount(int userId)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConnString"].ConnectionString))
            {
                var query = "SELECT TOP 1 * FROM BankAccounts where OwnerId = @userid";
                var any = connection.Query<BankAccountDto>(query, new { userid = userId }).AsList();
                if (any.Count == 0)
                {
                    return null;
                }
                else
                {
                    return any.FirstOrDefault();
                }

            }
        }

        public static bool InternalTransfer(AccountActionDto accountAction)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConnString"].ConnectionString))
            {
                using (var transactionScope = new TransactionScope())
                {
                    var querySelf = "update BankAccounts set Balance = (Balance - @amount) OUTPUT INSERTED.BankAccountId where BankAccountNumber = @bankId";
                    var queryDest = "update BankAccounts set Balance = (Balance + @amount) OUTPUT INSERTED.BankAccountId where BankAccountNumber = @bankId";
                    var insertAction = "insert into AccountActions OUTPUT INSERTED.AccountActionId values(@actionType, @title, @sourceName, @destName, @sourceNumber, @destNumber, @amount)";
                    var insertHistory = "insert into BankAccountHistory OUTPUT INSERTED.BankAccountHistoryId values(@actionId, @bankId, (select Balance from BankAccounts where BankAccountNumber = @bankNumber), @date)";
                    var updateSelfResult = connection.Query<int>(querySelf, new { bankId = accountAction.SourceBankNumber, amount = accountAction.Amount }).AsList();
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
                    var insertSelfHistory = connection.Query<int>(insertHistory, new { actionId = insertActionResult.First(), bankId = updateSelfResult.First(), bankNumber = accountAction.SourceBankNumber, date = DateTime.Now });
                    var insertDestHistory = connection.Query<int>(insertHistory, new { actionId = insertActionResult.First(), bankId = updateDestResult.First(), bankNumber = accountAction.DestinationBankNumber, date = DateTime.Now });
                    if (insertDestHistory.First() != 0 && insertSelfHistory.First() != 0)
                    {
                        transactionScope.Complete();
                        return true;
                    }

                    return false;

                }

            }
        }

        public static BoolResultDto ReceiveTransfer (AccountActionDto accountAction)
        {
            var result = new BoolResultDto();

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConnString"].ConnectionString))
            {
                using (var transactionScope = new TransactionScope())
                {
                    var query = "update BankAccounts set Balance = (Balance + @amount) output inserted.BankAccountId where BankAccountNumber = @bankId";
                    var insertAction = "insert into AccountActions OUTPUT INSERTED.AccountActionId values(@actionType, @title, @sourceName, @destName, @sourceNumber, @destNumber, @amount)";
                    var insertHistory = "insert into BankAccountHistory OUTPUT INSERTED.BankAccountHistoryId values(@actionId, @bankId, (select Balance from BankAccounts where BankAccountNumber = @bankNumber), @date)";
                    var updateResult = connection.Query<int>(query, new { bankId = accountAction.DestinationBankNumber, amount = accountAction.Amount });
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
                    var insertSelfHistory = connection.Query<int>(insertHistory, new { actionId = insertActionResult.First(), bankId = query.First(), bankNumber = accountAction.DestinationBankNumber, date = DateTime.Now });
                    if(insertSelfHistory.First() != 0)
                    {
                        transactionScope.Complete();
                        result.SetSuccess(true);
                    }
                }
                return result;
            }
               
        }

        public static BoolResultDto ExternalTransfer(AccountActionDto accountAction)
        {
            var result = new BoolResultDto();

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConnString"].ConnectionString))
            {
                using (var transactionScope = new TransactionScope())
                {
                    var data = Newtonsoft.Json.JsonConvert.SerializeObject(accountAction);
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var content = new StringContent(data, Encoding.UTF8);
                    HttpResponseMessage response = client.PostAsync(accountAction.Url, content).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                    else
                    {
                        var query = "update BankAccounts set Balance = (Balance - @amount) output inserted.BankAccountId where BankAccountNumber = @bankId";
                        var insertAction = "insert into AccountActions OUTPUT INSERTED.AccountActionId values(@actionType, @title, @sourceName, @destName, @sourceNumber, @destNumber, @amount)";
                        var insertHistory = "insert into BankAccountHistory OUTPUT INSERTED.BankAccountHistoryId values(@actionId, @bankId, (select Balance from BankAccounts where BankAccountNumber = @bankNumber), @date)";
                        var updateResult = connection.Query<int>(query, new { bankId = accountAction.DestinationBankNumber, amount = accountAction.Amount });
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
                        var insertSelfHistory = connection.Query<int>(insertHistory, new { actionId = insertActionResult.First(), bankId = query.First(), bankNumber = accountAction.DestinationBankNumber, date = DateTime.Now });
                        if (insertSelfHistory.First() != 0)
                        {
                            transactionScope.Complete();
                            result.SetSuccess(true);
                        }
                    }
                }
                return result;
            }

        }

    }
}
