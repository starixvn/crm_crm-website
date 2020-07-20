using System.Collections.Generic;

namespace Fanex.CRM.Models
{
    public class BaseReport<T>
    {
        public IEnumerable<T> DataList { get; set; }

        public int Totals { get; set; }
    }
}