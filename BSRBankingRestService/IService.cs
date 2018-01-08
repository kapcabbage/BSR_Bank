using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BSRBankingRestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
    [ServiceContract]
    public interface IExternalBanking
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            UriTemplate ="accounts/{bankAccountNumber}/History",
            ResponseFormat = WebMessageFormat.Json)]
        string RecieveTransfer(string bankAccountNumber, Stream content);
    }
}
