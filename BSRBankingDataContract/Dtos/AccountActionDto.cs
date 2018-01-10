using BSRBankingDataContract.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BSRBankingDataContract.Dtos
{
    [DataContract]
    public class AccountActionDto
    {
        [DataMember]
        public int AccountActionId { get; set; }
        public eActionType ActionType {get;set;}
        [DataMember]
        [JsonProperty("destination_name")]
        public string DestinationName { get; set; }
        [DataMember]
        [JsonProperty("source_name")]
        public string SourceName { get; set; }
        [DataMember]
        public string DestinationBankNumber { get; set; }
        [DataMember]
        [JsonProperty("source_account")]
        public string SourceBankNumber { get; set; }
        [DataMember]
        [JsonProperty("title")]
        public string Title { get; set; }
        [DataMember]
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [DataMember]
        public string Url { get; set; }
    }
}
