using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BSRBankingDataContract.Enums
{
    [DataContract]
    public enum eActionType
    {
        [EnumMember]
        Deposit = 1,
        [EnumMember]
        Withdrawal = 2,
        [EnumMember]
        InternalTransfer =3,
        [EnumMember]
        ExternalTranser = 4
    }
}
