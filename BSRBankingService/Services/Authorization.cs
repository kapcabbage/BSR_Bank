using BSRBankingDataContract.Base;
using BSRBankingService.Contracts;
using System;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using BSRBankingDataContract.Dtos;

namespace BSRBankingService.Services
{
    public partial class Service : IAuthorization
    {
        public UserResultDto AuthenticateUser(string userName, string passwordHash)
        {
            var result = new UserResultDto();
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BSRBanking"].ConnectionString))
                {
                    var any = connection.Query<UserDto>("SELECT COUNT(1) FROM Users ");

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
