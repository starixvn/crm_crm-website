namespace Fanex.CRM.Remark.Models
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Remarks
    {
        public int CustId { get; set; }

        public IEnumerable<string> SpecialRemarks { get; set; }

        public IEnumerable<string> NormalRemarks { get; set; }
    }
}