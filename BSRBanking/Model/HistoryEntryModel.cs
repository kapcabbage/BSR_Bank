using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSRBanking.Model
{
    public class HistoryEntryModel
    {
        public string Title { get; set; }
        public string SourceName { get; set; }
        public string SourceBankNumber { get; set; }
        public string DestinationName { get; set; }
        public string DestinationBankNumber { get; set; }
        public double Amount { get; set; }
        public double BalanceAfter { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}
