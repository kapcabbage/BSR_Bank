using BSRBankingDataAccess;
using BSRBankingDataContract.Dtos;
using BSRBankingDataContract.Enums;
using BSRBankingRestApi.Authorization;
using Newtonsoft.Json;
using System;
using System.Web.Http;

namespace BSRBankingRestApi.Controllers
{
    public class ExternalTransferController : ApiController
    {
        [HttpPost]
        [BasicAuthentication]
        [Route("accounts/{accountNumber}/history")]
        public IHttpActionResult Post(string accountNumber, [FromBody]object input)
        {
            try
            {
                AccountActionDto dto = JsonConvert.DeserializeObject<AccountActionDto>(input.ToString());
                dto.DestinationBankNumber = accountNumber;
                dto.ActionType = eActionType.ExternalTranser;
                if (dto.DestinationName == null || dto.DestinationName == string.Empty)
                {
                    return BadRequest("Destination name cannot be empty");
                }
                if (dto.SourceBankNumber == null || dto.SourceBankNumber == string.Empty)
                {
                    return BadRequest("Source bank number can not be empty");
                }
                if (dto.SourceName == null || dto.SourceName == string.Empty)
                {
                    return BadRequest("Source name can't be empty");
                }
                if (dto.Amount < 0)
                {
                    return BadRequest("Amount can't be lower then 0");
                }
                if (dto.Title == null || dto.Title == string.Empty)
                {
                    return BadRequest("Title can't be empty");
                }

                var result = BankAccount.ReceiveTransfer(dto);
                if (result.Success())
                {
                    return Created<AccountActionDto>("", null);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest("Unexpected error");

        }
    }
}