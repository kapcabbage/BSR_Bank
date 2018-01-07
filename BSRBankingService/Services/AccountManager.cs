using BSRBankingDataContract.Base;
using BSRBankingDataContract.Dtos;
using BSRBankingService.Contracts;
using System;
using System.Configuration;
using System.Data.SqlClient;
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
    }
}
