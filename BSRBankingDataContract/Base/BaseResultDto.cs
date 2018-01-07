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

        public void SetSuccess(T data)
        {
            Data = data;
            Result.Status = eOperationStatus.Success;
        }

        public void SetErrors(Exception exception)
        {
            Result.SetErrors(exception);
        }

        public void SetStatus(eOperationStatus status)
        {
            Result.Status = status;
        }

        public bool Success()
        {
            return Result.Status == eOperationStatus.Success;
        }

        public bool AccessDenied()
        {
            return Result.Status == eOperationStatus.AccessDenied;
        }


    }

    [DataContract(IsReference = true)]
    public class UserResultDto : BaseResultDto<UserDto>
    {

    }

    [DataContract(IsReference = true)]
    public class AccountActionListDto : BaseResultDto<List<AccountActionDto>>
    {

    }

    [DataContract(IsReference = true)]
    public class BankResultDto : BaseResultDto<BankAccountDto>
    {

    }
}
