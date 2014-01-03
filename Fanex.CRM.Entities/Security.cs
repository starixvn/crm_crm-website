
namespace Fanex.CRM.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Security
    {
       
        public string FileNames
        {
            get;
            set;
        }

        public string FuncName
        { get; set; }

        public int ActualPolicy
        { get; set; }
    }
}
