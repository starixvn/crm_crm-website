namespace Fanex.CRM.Common.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> source, int pageIndex, int itemsPerPage)
        {
            this.TotalItems = source.Count();

            this.ItemsPerPage = itemsPerPage;
            this.PageIndex = pageIndex;
            this.AddRange(source.Skip(pageIndex * itemsPerPage).Take(itemsPerPage));
        }

        public int PageIndex { get; set; }

        public int ItemsPerPage { get; set; }

        public int TotalItems { get; set; }
    }
}