using BSRBankingDataContract.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSRBankingDataContract.Dtos
{
    public class AccountActionDto
    {
        public int AccountActionId { get; set; }
        public eActionType ActionType {get;set;}
        [JsonProperty("destination_name")]
        public string DestinationName { get; set; }
        [JsonProperty("source_name")]
        public string SourceName { get; set; }
        public string DestinationBankNumber { get; set; }
        [JsonProperty("source_account")]
        public string SourceBankNumber { get; set; }
        public string Title { get; set; } 
        public int Amount { get; set; }
    }
}
