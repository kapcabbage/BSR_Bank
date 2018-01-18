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
using System.Net.Http.Headers;
using BSRBankingDataAccess.Context;

namespace BSRBankingDataAccess
{
    public static class BankAccount
    {
        public static List<HistoryEntryDto> GetHistory(int bankAccountId)
        {
            using (var connection = DbContext.SimpleDbConnection())
            {
                var query = "select * from History_V where BankAccountId = @bankId";
                var history = connection.Query<HistoryEntryDto>(query, new { bankId = bankAccountId }).AsList();
                if (history != null)
                {
                    connection.Close();
                    return history;
                }
                else connection.Close(); return new List<HistoryEntryDto>();
            }
        }

        public static BankAccountDto GetBankAccount(int userId)
        {
            using (var connection = DbContext.SimpleDbConnection())
            {
                var query = "SELECT * FROM BankAccounts where OwnerId = @userid limit 1";
                var any = connection.Query<BankAccountDto>(query, new { userid = userId }).AsList();
                if (any.Count == 0)
                {
                    connection.Close();
                    return null;
                }
                else
                {
                    connection.Close();
                    return any.FirstOrDefault();
                }

            }
        }

        public static BoolResultDto InternalTransfer(AccountActionDto accountAction)
        {
            using (var connection = DbContext.SimpleDbConnection())
            {
                var result = new BoolResultDto();
                connection.Open();
                var queryBalance = "select Balance from BankAccounts where BankAccountNumber  = @BankId";
                var queryBalanceResult = connection.Query<int>(queryBalance, new { BankId = accountAction.SourceBankNumber });
                if (queryBalanceResult.First() > accountAction.Amount && accountAction.Amount != 0)
                {
                    using (var transactionScope = connection.BeginTransaction())
                    {
                        var querySelf = "update BankAccounts set Balance = (Balance - @amount) where BankAccountNumber = @bankId;select BankAccountId from BankAccounts where BankAccountNumber = @bankId";
                        var queryDest = "update BankAccounts set Balance = (Balance + @amount)  where BankAccountNumber = @bankId;select BankAccountId from  BankAccounts where BankAccountNumber = @bankId";
                        var insertAction = "insert into AccountActions values(null,@actionType, @title, @sourceName, @destName, @sourceNumber, @destNumber, @amount);select last_insert_rowid()";
                        var insertHistory = "insert into BankAccountHistory values(null,@actionId, @bankId, (select Balance from BankAccounts where BankAccountNumber = @bankNumber), @date);select last_insert_rowid()";
                        var updateSelfResult = connection.Query<int>(querySelf, new { bankId = accountAction.SourceBankNumber, amount = accountAction.Amount });
                        var updateDestResult = connection.Query<int>(queryDest, new { bankId = accountAction.DestinationBankNumber, amount = accountAction.Amount });
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
                            transactionScope.Commit();
                            result.SetSuccess(true);

                        }
                        else
                        {
                            transactionScope.Rollback();
                            result.SetErrors("");
                        }
                        connection.Close();
                        return result;

                    }
                }
                else
                {
                    result.SetErrors("Invalid amount");
                    return result;
                }
                

            }
        }

        public static BoolResultDto ReceiveTransfer (AccountActionDto accountAction)
        {
            var result = new BoolResultDto();

            using (var connection = DbContext.SimpleDbConnection())
            {
                connection.Open();
                using (var transactionScope = connection.BeginTransaction())
                {
                    var query = "update BankAccounts set Balance = (Balance + @amount) where BankAccountNumber = @bankId;select BankAccountId from BankAccounts where BankAccountNumber = @bankId";
                    var insertAction = "insert into AccountActions values(null,@actionType, @title, @sourceName, @destName, @sourceNumber, @destNumber, @amount);select last_insert_rowid()";
                    var insertHistory = "insert into BankAccountHistory  values(null,@actionId, @bankId, (select Balance from BankAccounts where BankAccountNumber = @bankNumber), @date);select last_insert_rowid()";
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
                    var insertSelfHistory = connection.Query<int>(insertHistory, new { actionId = insertActionResult.First(), bankId = updateResult.First(), bankNumber = accountAction.DestinationBankNumber, date = DateTime.Now });
                    if(insertSelfHistory.First() != 0)
                    {
                        transactionScope.Commit();
                        result.SetSuccess(true);
                    }
                    else
                    {
                        transactionScope.Rollback();
                        result.SetErrors("Cannot transfer");
                    }
                }
                connection.Close();
                return result;
            }
               
        }

        public static BoolResultDto ExternalTransfer(AccountActionDto accountAction)
        {
            var result = new BoolResultDto();

            using (var connection = DbContext.SimpleDbConnection())
            {
                connection.Open();
                connection.Open();
                var queryBalance = "select Balance from BankAccounts where BankAccountNumber  = @BankId";
                var queryBalanceResult = connection.Query<int>(queryBalance, new { BankId = accountAction.SourceBankNumber });
                if (queryBalanceResult.First() > accountAction.Amount && accountAction.Amount != 0)
                {
                    using (var transactionScope = connection.BeginTransaction())
                    {
                        var data = Newtonsoft.Json.JsonConvert.SerializeObject(accountAction);
                        HttpClient client = new HttpClient();
                        var sad = ConfigurationManager.AppSettings["BasicAuthenticationUserName"];
                        var asz = ConfigurationManager.AppSettings["BasicAuthenticationPassword"];
                        var asd = string.Format("{0}:{1}", ConfigurationManager.AppSettings["BasicAuthenticationUserName"], ConfigurationManager.AppSettings["BasicAuthenticationPassword"]);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", ConfigurationManager.AppSettings["BasicAuthenticationUserName"], ConfigurationManager.AppSettings["BasicAuthenticationPassword"]))));
                        var content = new StringContent(data, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = client.PostAsync(accountAction.Url, content).Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception(response.ReasonPhrase);
                        }
                        else
                        {
                            var query = "update BankAccounts set Balance = (Balance - @amount) where BankAccountNumber = @bankId;select BankAccountId from BankAccounts where BankAccountNumber = @bankId";
                            var insertAction = "insert into AccountActions values(null,@actionType, @title, @sourceName, @destName, @sourceNumber, @destNumber, @amount);select last_insert_rowid()";
                            var insertHistory = "insert into BankAccountHistory values(null,@actionId, @bankId, (select Balance from BankAccounts where BankAccountNumber = @bankNumber), @date);select last_insert_rowid()";
                            var updateResult = connection.Execute(query, new { bankId = accountAction.DestinationBankNumber, amount = accountAction.Amount });
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
                                transactionScope.Commit();
                                result.SetSuccess(true);
                            }
                            else
                            {
                                transactionScope.Rollback();
                            }
                        }
                    }
                    connection.Close();
                    return result;
                }
                else
                {
                    result.SetErrors("Invalid value");
                    return result;
                }
            }
           

        }

        public static BoolResultDto Withdraw(AccountActionDto accountAction)
        {
            using (var connection = DbContext.SimpleDbConnection())
            {
                var result = new BoolResultDto();
                connection.Open();
                var queryBalance = "select Balance from BankAccounts where BankAccountNumber  = @BankId";
                var queryBalanceResult = connection.Query<int>(queryBalance, new { BankId = accountAction.SourceBankNumber });
                if (queryBalanceResult.First() > accountAction.Amount && accountAction.Amount != 0)
                {
                    using (var transactionScope = connection.BeginTransaction())
                    {
                        var querySelf = "update BankAccounts set Balance = (Balance - @amount) where BankAccountNumber = @bankId;select BankAccountId from BankAccounts where BankAccountNumber = @bankId";
                        var insertAction = "insert into AccountActions values(null,@actionType, @title, @sourceName, @destName, @sourceNumber, @destNumber, @amount);select last_insert_rowid()";
                        var insertHistory = "insert into BankAccountHistory values(null,@actionId, @bankId, (select Balance from BankAccounts where BankAccountNumber = @bankNumber), @date);select last_insert_rowid()";
                        var updateSelfResult = connection.Query<int>(querySelf, new { bankId = accountAction.SourceBankNumber, amount = accountAction.Amount });
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
                        if (insertSelfHistory.First() > 0)
                        {
                            transactionScope.Commit();
                            result.SetSuccess(true);

                        }
                        else
                        {
                            transactionScope.Rollback();
                            result.SetErrors("");
                        }
                        connection.Close();
                        return result;

                    }
                }
                else
                {
                    result.SetErrors("Invalid amount");
                    return result;
                }


            }
        }

        public static BoolResultDto Deposit(AccountActionDto accountAction)
        {
            using (var connection = DbContext.SimpleDbConnection())
            {
                var result = new BoolResultDto();
                connection.Open();
                var queryBalance = "select Balance from BankAccounts where BankAccountNumber  = @BankId";
                var queryBalanceResult = connection.Query<int>(queryBalance, new { BankId = accountAction.SourceBankNumber });
                if (accountAction.Amount > 0)
                {
                    using (var transactionScope = connection.BeginTransaction())
                    {
                        var querySelf = "update BankAccounts set Balance = (Balance + @amount) where BankAccountNumber = @bankId;select BankAccountId from BankAccounts where BankAccountNumber = @bankId";
                        var insertAction = "insert into AccountActions values(null,@actionType, @title, @sourceName, @destName, @sourceNumber, @destNumber, @amount);select last_insert_rowid()";
                        var insertHistory = "insert into BankAccountHistory values(null,@actionId, @bankId, (select Balance from BankAccounts where BankAccountNumber = @bankNumber), @date);select last_insert_rowid()";
                        var updateSelfResult = connection.Query<int>(querySelf, new { bankId = accountAction.SourceBankNumber, amount = accountAction.Amount });
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
                        if (insertSelfHistory.First() > 0)
                        {
                            transactionScope.Commit();
                            result.SetSuccess(true);

                        }
                        else
                        {
                            transactionScope.Rollback();
                            result.SetErrors("");
                        }
                        connection.Close();
                        return result;

                    }
                }
                else
                {
                    result.SetErrors("Invalid amount");
                    return result;
                }


            }
        }

        public static ActionTypeResultDto CheckTransferType(string bankNumber)
        {
            var result = new ActionTypeResultDto();
            try
            {
                using (var connection = DbContext.SimpleDbConnection())
                {

                    var query = "select BankAccountId from BankAccounts where BankAccountNumber = @bankNumb";
                    var queryResult = connection.Query<int>(query, new { bankNumb = bankNumber });
                    if (queryResult != null && queryResult.FirstOrDefault() > 0)
                    {
                        result.SetSuccess(BSRBankingDataContract.Enums.eActionType.InternalTransfer);
                    }
                    else
                    {
                        result.SetSuccess(BSRBankingDataContract.Enums.eActionType.ExternalTranser);
                    }
                    connection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.SetErrors(ex.Message);
                return result;
            }

        }

        public static StringResultDto CheckExternalAccount(string externalBankNumber)
        {
            var result = new StringResultDto();
            var givenBankId = externalBankNumber.Substring(2, 8);
            using (var reader = new StreamReader(@"D:\Dokumenty\Pojects\BSRBanking\CSVBANK.csv"))
            {
                var list = new List<ExternalAccountDto>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    list.Add(new ExternalAccountDto()
                    {
                        BankId = values[0],
                        Url = values[1]
                    });
                }

                if(list.Any(x=>x.BankId == givenBankId))
                {
                    var url = list.FirstOrDefault(x=>x.BankId == givenBankId);
                    result.SetSuccess(url.Url+externalBankNumber+"/history");
                }
                else
                {
                    result.SetErrors("Account number not found");
                }
            }
            
            return result;
        }

    }
}
