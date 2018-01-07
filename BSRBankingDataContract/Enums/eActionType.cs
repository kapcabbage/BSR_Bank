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
        SelfTransfer = 1,
        [EnumMember]
        SelfWithdrawal = 2,
        [EnumMember]
        InternalTransfer =3,
        [EnumMember]
        ExternalTranser = 4
    }
}
