namespace Fanex.CRM.ActionLog.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public abstract class GetActionLogCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int ActionId { get; set; }

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int ItemsPerPage { get; set; }
    }
}