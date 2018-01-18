using BSRBankingDataAccess.Context;
using BSRBankingDataContract.Base;
using BSRBankingDataContract.Dtos;
using Dapper;
using System.Linq;

namespace BSRBankingDataAccess
{
    public static class Authorization
    {
        public static UserResultDto Authorize(string userName, string passwordHash)
        {
            var result = new UserResultDto();
            using (var connection = DbContext.SimpleDbConnection())
            {
                var query = "SELECT * FROM Users u inner join UsersPasswords p on p.UserId = u.UserId where u.UserName = @username and p.PasswordHash = @passwordhash LIMIT 1";
                var any = connection.Query<UserDto>(query, new { username = userName, passwordhash = passwordHash }).AsList();
                if (any.Count == 0)
                {
                    result.SetStatus(BSRBankingDataContract.Enums.eOperationStatus.AccessDenied);
                }
                else
                {
                    result.SetSuccess(any.FirstOrDefault());
                }
                return result;
            }
        }
    }
}
