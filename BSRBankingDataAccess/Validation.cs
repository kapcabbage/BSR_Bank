using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSRBankingDataAccess
{
    public static class Validation
    {
        public static bool ValidateNrb(string bankNumber)
        {
            var cc = bankNumber.Substring(0, 2);
            var bankId = bankNumber.Substring(2);
            var output = System.Numerics.BigInteger.Parse(bankId + "2521" + cc) % 97;
            if (bankNumber.Count() != 26 || !((System.Numerics.BigInteger.Parse(bankId+"2521"+cc) % 97) == 1))
            {
                return false;
            }
            return true;
        }
    }
}
