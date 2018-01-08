using BSRBankingDataContract.Dtos;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Script.Serialization;

namespace BSRBankingRestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service.svc or Service.svc.cs at the Solution Explorer and start debugging.
    public class Service : IExternalBanking
    {
        public string RecieveTransfer(string bank, Stream content)
        {
            string input = new StreamReader(content).ReadToEnd();
            //AccountActionDto dto = JsonConvert.DeserializeObject<AccountActionDto>(input);
            //dto.DestinationBankNumber = bank;
             
            return bank + input;
        }
    }
}
