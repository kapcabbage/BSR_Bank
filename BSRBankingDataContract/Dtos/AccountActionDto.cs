using BSRBankingDataContract.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSRBankingDataContract.Dtos
{
    public class AccountActionDto
    {
        public int AccountActionId { get; set; }
        public eActionType ActionType {get;set;}
        public string DestinationName { get; set; }
        public string SourceName { get; set; }
        public string DestinationBankNumber { get; set; }
        public string SourceBankNumber { get; set; }
        public string Title { get; set; } 
        public int Amount { get; set; }
    }
}
