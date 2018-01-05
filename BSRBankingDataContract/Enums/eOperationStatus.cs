using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BSRBankingDataContract.Enums
{
    [DataContract]
    public enum eOperationStatus
    {
        [EnumMember]
        Success = 1,

        [EnumMember]
        AccessDenied = 2,

        [EnumMember]
        GeneralError = 3,

        [EnumMember]
        NotDefined = 4,

    }
}
