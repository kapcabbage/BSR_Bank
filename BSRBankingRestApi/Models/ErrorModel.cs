using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BSRBankingRestApi.Models
{
    public class ErrorModel
    {
        public string Error { get; set; }
        public string Field { get; set; }

        public ErrorModel(string error, string field)
        {
            Error = error;
            Field = field;
        }
    }
}