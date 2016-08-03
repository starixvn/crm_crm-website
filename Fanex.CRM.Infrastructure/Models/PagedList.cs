// --------------------------------------------------------------------------
// <copyright file="PagedList.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.18">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Models
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Based class for pagination.
    /// </summary>
    /// <typeparam name="T">Paged List Type.</typeparam>
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
