using BSRBankingDataContract.Base;
using BSRBankingService.Contracts;
using System;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using BSRBankingDataContract.Dtos;
using System.Linq;
using BSRBankingDataAccess;

namespace BSRBankingService.Services
{
    public partial class Service : IAuthorization
    {
        public UserResultDto AuthenticateUser(string userName, string passwordHash)
        {
            var result = new UserResultDto();
            try
            {
                var user = Authorization.Authorize(userName, passwordHash);

                if (user.Success())
                {
                    result.SetSuccess(user.Data);
                }
                else
                {
                    result.SetErrors(user.Result.ExceptionMessage);
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
