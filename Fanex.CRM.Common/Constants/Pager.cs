namespace Fanex.CRM.Common.Constants
{
    using System.Collections.Generic;

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