namespace Fanex.CRM.Common.Models
{
    using Fanex.Data.Repository;

    public class PagingCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public virtual int PageSize { get; set; }

        [SpParam(Ignored = true)]
        public virtual int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int StartIndex
        {
            get
            {
                if (PageIndex <= 0)
                {
                    PageIndex = 1;
                }

                return (PageIndex - 1) * PageSize;
            }
        }

        [SpParam(Ignored = true)]
        public int EndIndex
        {
            get
            {
                var endIndex = StartIndex + PageSize - 1;
                return endIndex > 0 ? endIndex : 0;
            }
        }
    }
}