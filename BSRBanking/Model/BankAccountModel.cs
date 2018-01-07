using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSRBanking.Model
{
    public class BankAccountModel
    {
        public int BankAccountId { get; set; }
        public string BankAccountNumber { get; set; }
        public int Balance { get; set; }
    }
}
