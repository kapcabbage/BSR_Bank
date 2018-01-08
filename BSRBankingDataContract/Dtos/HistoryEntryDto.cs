using System;
using System.Collections.Generic;
using System.Text;

namespace BSRBankingDataContract.Dtos
{
    public class HistoryEntryDto
    {
        public string Title { get; set; }
        public string SourceName { get; set; }
        public string SourceBankNumber { get; set; }
        public string DestinationName { get; set; }
        public string DestinationBankNumber { get; set; }
        public int Amount { get; set; }
        public int BalanceAfter { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}
