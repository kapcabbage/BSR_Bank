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
using BSRBankingDataContract.Enums;

namespace BSRBankingService.Services
{
    public partial class Service : IAccountManager
    {
        public BoolResultDto Deposit(AccountActionDto action)
        {
            var result = new BoolResultDto();
            try
            {
                if (Validation.ValidateNrb(action.DestinationBankNumber))
                {
                    action.ActionType = BSRBankingDataContract.Enums.eActionType.Deposit;
                    var transferResult = BankAccount.Deposit(action);
                    if (transferResult.Success())
                    {
                        result = transferResult;
                    }
                }
                else
                {
                    result.SetErrors("Account number is invalid");
                }
            }
            catch (Exception ex)
            {
                result.SetErrors(ex);
            }
            return result;
        }

        public BoolResultDto External(AccountActionDto action)
        {
            var result = new BoolResultDto();
            try
            {
                if (Validation.ValidateNrb(action.DestinationBankNumber))
                {
                    action.ActionType = BSRBankingDataContract.Enums.eActionType.ExternalTranser;
                    var transferResult = BankAccount.ExternalTransfer(action);
                    result = transferResult;
                    
                }
                else
                {
                    result.SetErrors("Account number is invalid");
                }
            }
            catch (Exception ex)
            {
                result.SetErrors(ex);
            }
            return result;
        }

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
                if (Validation.ValidateNrb(accountAction.DestinationBankNumber))
                {
                    var transferType = BankAccount.CheckTransferType(accountAction.DestinationBankNumber);
                    if (transferType.Success() && transferType.Data == eActionType.InternalTransfer)
                    {
                        accountAction.ActionType = eActionType.InternalTransfer;
                        var transferResult = BankAccount.InternalTransfer(accountAction);
                        if (transferResult.Success())
                        {
                            result.SetSuccess(transferResult.Data);
                        }
                        else
                        {
                            result.SetErrors(transferResult.Result.ExceptionMessage);
                        }
                    }
                    else if (transferType.Success() && transferType.Data == eActionType.ExternalTranser)
                    {
                        var externalValidation = BankAccount.CheckExternalAccount(accountAction.DestinationBankNumber);
                        if (externalValidation.Success())
                        {
                            accountAction.Url = externalValidation.Data;
                            accountAction.ActionType = eActionType.ExternalTranser;
                            var transferResult = BankAccount.ExternalTransfer(accountAction);
                            if (transferResult.Success())
                            {
                                result.SetSuccess(transferResult.Data);
                            }
                            else
                            {
                                result.SetErrors(transferResult.Result.ExceptionMessage);
                            }
                        }
                        else
                        {
                            result.SetErrors(externalValidation.Result.ExceptionMessage);
                        }
                    }
                }
                else
                {
                    result.SetErrors("Account number is invalid");
                }
            }
            catch (Exception ex)
            {
                result.SetErrors(ex);
            }
            return result;
        }

        public BoolResultDto Withdraw(AccountActionDto action)
        {
            var result = new BoolResultDto();
            try
            {
                if (Validation.ValidateNrb(action.DestinationBankNumber))
                {
                    action.ActionType = BSRBankingDataContract.Enums.eActionType.Withdrawal;
                    var transferResult = BankAccount.Withdraw(action);
                    result = transferResult;
                }
                else
                {
                    result.SetErrors("Account number is invalid");
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
