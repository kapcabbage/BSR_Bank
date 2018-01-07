using System;
using System.Collections.Generic;
using System.Text;

namespace BSRBankingDataContract.Dtos
{
    public class BankAccountDto
    {
        public int BankAccountId { get; set; }
        public string BankAccountNumber { get; set; }
        public int Balance { get; set; }
    }
}
