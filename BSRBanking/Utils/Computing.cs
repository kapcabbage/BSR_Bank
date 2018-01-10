using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BSRBanking.Utils
{
    public static class Computing
    {
        public static string Sha256(string randomString)
        {
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
            System.Text.StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString), 0, Encoding.UTF8.GetByteCount(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        public static string ComputeChecksum(string input)
        {
            var bankId = input.Substring(0, 8);
            var accountNumber = input.Substring(8, 16);
            var output = bankId + accountNumber + "252100";
            try
            {
    
                var resultInt = 98 - (System.Numerics.BigInteger.Parse(output) % 97);
                return resultInt.ToString(); ;
            }
            catch (Exception ex)
            {
                return "";

            }
            

        }
    }
}
