using BSRBankingDataAccess;
using BSRBankingDataContract.Dtos;
using BSRBankingDataContract.Enums;
using BSRBankingRestApi.Authorization;
using BSRBankingRestApi.Models;
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
                var validationResult = Validation.ValidateNrb(accountNumber);
                var validateSource = Validation.ValidateNrb(dto.SourceBankNumber);
                if (!(validationResult && validateSource))
                {
                    var error = new ErrorModel("Account number invalid","AccountNumber");
                    return BadRequest(JsonConvert.SerializeObject(error));
                }
                if (dto.DestinationName == null || dto.DestinationName == string.Empty)
                {
                    var error = new ErrorModel("Destination name cannot be empty", "DestinationName");
                    return BadRequest(JsonConvert.SerializeObject(error));
                }
                if (dto.SourceName == null || dto.SourceName == string.Empty)
                {
                    var error = new ErrorModel("Source name cannot be empty", "SourceName");
                    return BadRequest(JsonConvert.SerializeObject(error));
                }
                if (dto.Amount < 0)
                {
                    var error = new ErrorModel("Amount cannot be lower than 0", "Amount");
                    return BadRequest(JsonConvert.SerializeObject(error));
                }
                if (dto.Title == null || dto.Title == string.Empty)
                {
                    var error = new ErrorModel("Title cannot be empty", "Title");
                    return BadRequest(JsonConvert.SerializeObject(error));
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