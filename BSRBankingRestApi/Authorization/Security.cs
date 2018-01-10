using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BSRBankingRestApi.Authorization
{
    public static class Security
    {
        public static bool ValidateUser(string username, string password)
        {
            if(username.Equals((ConfigurationManager.AppSettings["BasicAuthenticationUserName"])) && password.Equals(ConfigurationManager.AppSettings["BasicAuthenticationPassword"])){
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}