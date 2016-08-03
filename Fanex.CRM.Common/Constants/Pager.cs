// --------------------------------------------------------------------------
// <copyright file="Pager.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.12.09">Created</change>
// </history>

namespace Fanex.CRM.Common.Constants
{
    using System.Collections.Generic;

    /// <summary>
    /// Pager class.
    /// </summary>
    public static class Pager
    {
        public const int DefaultItemsPerPage = 50;

        public const int DefaultLargeItemsPerPage = 100;

        public static List<int> ItemsPerPageArray
        {
            get
            {
                return new List<int> { 50, 100, 200 };
            }
        }

        public static List<int> LargeItemsPerPageArray
        {
            get
            {
                return new List<int> { 100, 200, 300, 400, 500 };
            }
        }
    }
}
