using BSRBankingDataContract.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BSRBankingDataContract.Base
{
    [DataContract(IsReference = true)]
    public class OperationResult
    {
        [DataMember]
        public eOperationStatus Status { get; set; }

        [DataMember]
        public string ExceptionMessage { get; set; }


        public OperationResult()
        {
            Status = eOperationStatus.NotDefined;
        }

        public void SetErrors(Exception exception)
        {
            ExceptionMessage = exception.Message;
            Status = eOperationStatus.GeneralError;
        }
    }
}
