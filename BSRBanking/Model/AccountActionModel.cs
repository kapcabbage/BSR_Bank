using BSRBanking.Service;
using BSRBankingDataContract.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BSRBanking.Model
{
    public class AccountActionModel
    {
        public eActionType ActionType { get; set; }
        public string DestinationName { get; set; }
        public string SourceName { get; set; }
        public string DestinationBankNumber { get; set; }
        public string SourceBankNumber { get; set; }
        public string Title { get; set; }
        public int Amount { get; set; }

        public AccountActionModel(eActionType type, string destName, string destNum, string sourceName, string sourceNum, string title, int amount)
        {
            ActionType = type;
            DestinationName = destName;
            DestinationBankNumber = destNum;
            SourceName = sourceName;
            SourceBankNumber = sourceNum;
            Title = title;
            Amount = amount;
        }
    }
}
