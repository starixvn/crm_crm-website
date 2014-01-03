

namespace Fanex.CRM.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Result
    {
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public Result()
        {

        }

        public Result(int errCode, string errMsg)
        {
            ErrCode = errCode;
            ErrMsg = errMsg;
        }

        public bool IsSuccessfull
        {
            get { return ErrCode == 0 && string.IsNullOrEmpty(ErrMsg); }
        }
    }
}
