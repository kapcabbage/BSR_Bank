﻿using BSRBankingDataContract.Base;
using BSRBankingService.Contracts;
using System;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using BSRBankingDataContract.Dtos;
using System.Linq;

namespace BSRBankingService.Services
{
    public partial class Service : IAuthorization
    {
        public UserResultDto AuthenticateUser(string userName, string passwordHash)
        {
            var result = new UserResultDto();
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConnString"].ConnectionString))
                {
                    var query = "SELECT TOP 1 * FROM Users u inner join UsersPasswords p on p.UserId = u.UserId where u.UserName = @username and p.PasswordHash = @passwordhash";
                    var any = connection.Query<UserDto>(query,new { username = userName, passwordhash = passwordHash}).AsList();
                    if (any.Count == 0)
                    {
                        result.SetStatus(BSRBankingDataContract.Enums.eOperationStatus.AccessDenied);
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
    }
}
