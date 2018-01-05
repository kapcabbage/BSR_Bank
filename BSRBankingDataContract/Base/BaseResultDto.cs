using BSRBankingDataContract.Dtos;
using BSRBankingDataContract.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BSRBankingDataContract.Base
{
    [DataContract(IsReference = true)]
    public class BaseResultDto<T>
    {
        [DataMember]
        public T Data { get; set; }

        [DataMember]
        public OperationResult Result { get; set; }


        public BaseResultDto()
        {
            Data = default(T);
            Result = new OperationResult();
        }

        public void SetErrors(Exception exception)
        {
            Result.SetErrors(exception);
        }

        public void SetStatus(eOperationStatus status)
        {
            Result.Status = status;
        }



    }

    [DataContract(IsReference = true)]
    public class UserResultDto : BaseResultDto<UserDto>
    {

    }
}
