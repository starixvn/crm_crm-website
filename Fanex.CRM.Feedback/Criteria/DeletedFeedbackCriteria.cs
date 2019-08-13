namespace Fanex.CRM.Feedback.Criteria
{
    using System;
    using System.Collections;
    using Common.Criteria;

    public class DeletedFeedbackCriteria : BasedCriteria
    {
        public string Username { get; set; }

        public int IsGuest { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int MainCategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public int CommunicationId { get; set; }

        public int ResponseId { get; set; }

        public int IsReviewed { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public bool IsExportExcel { get; set; }

        public override string GetSettingKey()
        {
            return "FeedbackGetDeleted";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate && (ToDate - FromDate).Days <= 365;
        }

        public override Hashtable GetSpParams()
        {
            var inputParams = new Hashtable();
            inputParams.Add("fromdate", FromDate);
            inputParams.Add("todate", ToDate);
            inputParams.Add("userrole", UserRole);

            inputParams.Add("username", Username);
            if (IsGuest >= 0)
            {
                inputParams.Add("isguest", IsGuest == 1);
            }

            if (MainCategoryId > 0)
            {
                inputParams.Add("maincategoryid", MainCategoryId);
            }

            if (SubCategoryId > 0)
            {
                inputParams.Add("subcategoryid", SubCategoryId);
            }

            if (CommunicationId > 0)
            {
                inputParams.Add("communicationid", CommunicationId);
            }

            if (ResponseId > 0)
            {
                inputParams.Add("responseid", ResponseId);
            }

            //// 0 : All
            //// 1 : Reviewed
            //// -1: Not reviewed
            if (IsReviewed == 1)
            {
                inputParams.Add("isreviewed", true);
            }
            else if (IsReviewed == -1)
            {
                inputParams.Add("isreviewed", false);
            }

            return inputParams;
        }
    }
}